using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using BAFactory.Fx.Network.Email;
using BAFactory.Fx.Utilities.Encoding;

namespace BAFactory.Fx.Utilities.Email
{
    public class EmailParser
    {
        private const string NEWLINE = "\r\n";
        private const string VALIDHEADERPATTERN = "^[\t]*(.*?)[:=](.*)$";

        public EmailParser()
        {
        }

        public EMailMessage CreateEmailFromRawText(string RawEmail)
        {
            EMailMessage result = new EMailMessage();
            ParseEMailMessage(ref result, RawEmail);
            return result;
        }

        public void ParseEMailMessageHeader(ref EMailMessage OriginalEmail, string RawEMail)
        {
            ParseEMailMessageHeader(ref OriginalEmail, RawEMail, true);
        }
        public void ParseEMailMessageHeader(ref EMailMessage OriginalEmail, string RawEMail, bool Overwrite)
        {
            string[] messageLines = ExtractMessageLines(RawEMail);
            int endHeaderPos = FindHeaderEnd(messageLines);

            string MessageBoundary = string.Empty;

            ParseMessageHeaders(ref OriginalEmail, GetHeaderLines(messageLines, endHeaderPos), out MessageBoundary);
        }

        public void ParseEMailMessage(ref EMailMessage OriginalEmail, string RawEMail)
        {
            ParseEMailMessage(ref OriginalEmail, RawEMail, true);
        }
        public void ParseEMailMessage(ref EMailMessage OriginalEmail, string RawEMail, bool Overwrite)
        {
            //QuotedTextEncoder decoder = new QuotedTextEncoder();
            //string decoded = decoder.DecodeFromQuotedPrintable(RawEMail);
            //decoded = RemoveInvalidChars(decoded);

            string[] messageLines = ExtractMessageLines(RawEMail);
            int endHeaderPos = FindHeaderEnd(messageLines);

            string messageBoundary = string.Empty;

            ParseMessageHeaders(ref OriginalEmail, GetHeaderLines(messageLines, endHeaderPos), out messageBoundary);

            int[] boundariesPos;

            boundariesPos = FindBoundariesPositions(messageLines, messageBoundary, endHeaderPos);

            ParseMessageContent(ref OriginalEmail, GetBodyLines(messageLines, endHeaderPos), boundariesPos);
        }

        public string GetEMailAddressTag(EMailAddress Address)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(Address.DisplayName) &&
                !string.IsNullOrEmpty(Address.Address))
            {
                result = Address.Address;
            }
            else
            {
                if (string.IsNullOrEmpty(Address.Address))
                {
                    result = Address.DisplayName;
                }
                else
                {
                    result = string.Format("{0} <{1}>", Address.DisplayName, Address.Address);
                }
            }
            return result;
        }

        private string[] ExtractMessageLines(string RawEMail)
        {
            Regex rxLines = new Regex(@"(.*)\r\n");
            MatchCollection mLines = rxLines.Matches(RawEMail);

            string[] result = new string[mLines.Count];

            for (int i = 0; i < mLines.Count; ++i)
            {
                result[i] = mLines[i].Value.Remove(mLines[i].Value.Length - 2, 2);
            }

            return result;
        }

        private void ParseMessageHeaders(ref EMailMessage OriginalEmail, string[] MessageHeaderLines, out string MessageBoundary)
        {
            ParseMessageHeaders(ref OriginalEmail, MessageHeaderLines, out MessageBoundary, false);
        }
        private void ParseMessageHeaders(ref EMailMessage OriginalEmail, string[] MessageHeaderLines, out string MessageBoundary, bool decodeText)
        {
            StringBuilder rawHeader = new StringBuilder();
            foreach (string line in MessageHeaderLines)
            {
                rawHeader.Append(string.Concat(line, NEWLINE));
            }
            OriginalEmail.Header = rawHeader.ToString();

            Regex validHeaderLineRegEx = new Regex(VALIDHEADERPATTERN);
            Match validHeaderLineMatch;

            MessageBoundary = string.Empty;

            string[] fixedMessageHeaderLines = SplitMultiValueLines(MessageHeaderLines);
            bool continuesFromPrevious = false;
            string lastHeaderProcessed = string.Empty;
            string headerName = string.Empty;
            string headerValue = string.Empty;

            foreach (string line in fixedMessageHeaderLines)
            {
                continuesFromPrevious = (line.StartsWith("\t") || line.StartsWith(" "));
                validHeaderLineMatch = validHeaderLineRegEx.Match(line);

                if (continuesFromPrevious && (CheckValidDateString(line) || lastHeaderProcessed == "to" || lastHeaderProcessed == "cc"))
                {
                    headerName = lastHeaderProcessed;
                    headerValue = line.Trim();
                }
                else
                {
                    headerName = validHeaderLineMatch.Groups[1].Value.Trim().ToLower();
                    headerValue = validHeaderLineMatch.Groups[2].Value.Trim();
                }

                switch (headerName)
                {
                    case "subject":
                        OriginalEmail.Subject = headerValue;
                        break;
                    case "from":
                        OriginalEmail.From = ParseEmailAddress(headerValue);
                        break;
                    case "to":
                        EMailAddress[] newAddresses = ParseEmailAddressTag(headerValue);
                        EMailAddress[] patchedList = new EMailAddress[OriginalEmail.To.Length + newAddresses.Length];
                        OriginalEmail.To.CopyTo(patchedList, 0);
                        newAddresses.CopyTo(patchedList, OriginalEmail.To.Length);
                        OriginalEmail.To = patchedList;
                        break;
                    case "received":
                    case "date":
                        string cleanHeader = GetCleanValue(headerValue);
                        string dateString = GetValidDateString(cleanHeader);
                        if (dateString != string.Empty)
                        {
                            OriginalEmail.Date = dateString;
                        }
                        break;
                    case "priority":
                    case "importance":
                        OriginalEmail.Priority = GetCleanValue(headerValue);
                        break;
                    case "reply-to":
                        OriginalEmail.ReplyTo = ParseEmailAddress(headerValue);
                        break;
                    case "content-class":
                        OriginalEmail.ContentClasses = GetCleanValue(headerValue);
                        break;
                    case "mime-version":
                        OriginalEmail.MIMEVersion = GetCleanValue(headerValue);
                        break;
                    case "content-type":
                        OriginalEmail.ContentType = GetCleanValue(headerValue);
                        break;
                    case "content-transfer-encoding":
                        OriginalEmail.ContentTransferEncoding = GetCleanValue(headerValue);
                        break;
                    case "cc":
                        OriginalEmail.CC = ParseEmailAddressTag(headerValue);
                        break;
                    case "subject-encoding":
                        OriginalEmail.SubjectEncoding = GetCleanValue(headerValue);
                        break;
                    case "body-encoding":
                        OriginalEmail.BodyEncoding = GetCleanValue(headerValue);
                        break;
                    case "boundary":
                        string cleanBoundary = GetCleanValue(headerValue);
                        MessageBoundary = string.Format("--{0}", cleanBoundary);
                        break;
                    default:
                        OriginalEmail.OtherHeaders.Add(headerValue);
                        break;
                }
                lastHeaderProcessed = headerName;
            }
        }
        private void ParseMessageContent(ref EMailMessage OriginalEmail, string[] MessageContentLines, int[] BoundariesPos)
        {
            int bodyAttachsCount = 0;
            EMailBodyAlternateView refBody;
            string[] bodyLines;

            EMailBodyAlternateView[] views = null;
            EMailAttachment[] attachs = null;

            switch (OriginalEmail.ContentType.ToLower())
            {
                case null:
                    OriginalEmail.ContentType = "text/plain";
                    goto case "text/plain";
                case "text":
                    OriginalEmail.ContentType = "text/plain";
                    goto case "text/plain";
                case "text/plain":
                case "text/html":
                case "text/htm":
                case "":
                    OriginalEmail.Body = new EMailBodyAlternateView();
                    refBody = OriginalEmail.Body;

                    refBody.ContentTransferEncoding = OriginalEmail.ContentTransferEncoding;
                    refBody.ContentType = OriginalEmail.ContentType;

                    string messageLine;
                    if (OriginalEmail.ContentTransferEncoding == "base64")
                    {
                        messageLine = ConcatenateStringArray(GetStringArrayPart(MessageContentLines, 0, MessageContentLines.Length - 1), true);
                    }
                    else
                    {
                        messageLine = ConcatenateStringArray(MessageContentLines);
                    }

                    refBody.ContentStream = messageLine;
                    break;
                case "multipart/alternative":
                    ParseMultipartAlternativeViews(MessageContentLines, BoundariesPos, ref views, ref attachs);
                    OriginalEmail.Body = views[0];
                    if (views != null && views.Length > 1)
                    {
                        OriginalEmail.Views = new EMailBodyAlternateView[views.Length - 1];
                        EMailBodyAlternateView[] viewsRef = OriginalEmail.Views;
                        Array.Copy(views, 1, viewsRef, 0, views.Length - 1);
                    }
                    OriginalEmail.Attachments = attachs;
                    break;
                case "multipart/mixed":
                case "multipart/related":
                case "multipart/report":
                    if (BoundariesPos.Length > 2)
                    {
                        bodyLines = GetStringArrayPart(MessageContentLines, BoundariesPos[0], BoundariesPos[1]);
                        bodyAttachsCount = BoundariesPos.Length - 2;
                    }
                    else
                    {
                        bodyLines = GetStringArrayPart(MessageContentLines, BoundariesPos[0], BoundariesPos[1]);
                    }
                    ParseNestedContent(bodyLines, ref views, ref attachs);
                    OriginalEmail.Body = views[0];
                    OriginalEmail.Attachments = attachs;
                    if (views != null && views.Length > 1)
                    {
                        OriginalEmail.Views = new EMailBodyAlternateView[views.Length - 1];
                        EMailBodyAlternateView[] viewsRef = OriginalEmail.Views;
                        Array.Copy(views, 1, viewsRef, 0, views.Length - 1);
                    }
                    break;
            }

            if (bodyAttachsCount > 0)
            {
                int inlineAttachs = OriginalEmail.Attachments.Length;

                EMailAttachment[] emAttachments = new EMailAttachment[inlineAttachs + bodyAttachsCount];
                Array.Copy(OriginalEmail.Attachments, emAttachments, inlineAttachs);

                OriginalEmail.Attachments = emAttachments;

                for (int i = inlineAttachs, j = 0; i < inlineAttachs + bodyAttachsCount; ++i, j++)
                {
                    OriginalEmail.Attachments[i] = new EMailAttachment();

                    int startLine = BoundariesPos[1 + j];
                    int endLine = BoundariesPos[2 + j];

                    ParseAttachment(ref OriginalEmail.Attachments[i], GetStringArrayPart(MessageContentLines, startLine + 1, endLine - 1));
                }
            }
        }

        private void ParseAlternativeView(ref EMailBodyAlternateView AlternateView, string[] ViewLines)
        {
            int headerEndPos = FindHeaderEnd(ViewLines);
            ParseAlternateViewHeaders(ref AlternateView, GetHeaderLines(ViewLines, headerEndPos));
            ParseAlternateViewContent(ref AlternateView, GetBodyLines(ViewLines, headerEndPos));
        }
        private void ParseAlternateViewHeaders(ref EMailBodyAlternateView AlternateView, string[] ViewHeaderLines)
        {
            Regex validHeaderLineRegEx = new Regex(VALIDHEADERPATTERN);
            Match validHeaderLineMatch;
            ArrayList linkedRes = new ArrayList();

            string[] fixedMessageHeaderLines = SplitMultiValueLines(ViewHeaderLines);
            bool continuesFromPrevious = false;
            string lastHeaderProcessed = string.Empty;
            string headerName = string.Empty;
            string headerValue = string.Empty;

            foreach (string line in fixedMessageHeaderLines)
            {
                continuesFromPrevious = (line.StartsWith("\t") || line.StartsWith(" "));
                validHeaderLineMatch = validHeaderLineRegEx.Match(line);

                validHeaderLineMatch = validHeaderLineRegEx.Match(line);

                if (validHeaderLineMatch.Groups.Count < 3)
                {
                    continue;
                }

                if (continuesFromPrevious && CheckValidDateString(line))
                {
                    headerName = lastHeaderProcessed;
                    headerValue = line.Trim();
                }
                else
                {
                    headerName = validHeaderLineMatch.Groups[1].Value.Trim().ToLower();
                    headerValue = validHeaderLineMatch.Groups[2].Value.Trim();
                }

                switch (headerName)
                {
                    case "content-type":
                        AlternateView.ContentType = GetCleanValue(headerValue);
                        break;
                    case "content-transfer-encoding":
                        AlternateView.ContentTransferEncoding = GetCleanValue(headerValue);
                        break;
                    case "charset":
                        AlternateView.Charset = GetCleanValue(headerValue);
                        break;
                    case "base-uri":
                        AlternateView.BaseUri = GetCleanValue(headerValue);
                        break;
                    case "id":
                        AlternateView.Id = GetCleanValue(headerValue);
                        break;
                    case "linked-resources":
                        linkedRes.Add(GetCleanValue(headerValue));
                        break;
                }
            }
            if (linkedRes.Count > 0)
            {
                AlternateView.LinkedResources = new string[linkedRes.Count];
                linkedRes.CopyTo(AlternateView.LinkedResources);
            }

        }
        private void ParseAlternateViewContent(ref EMailBodyAlternateView AlternateView, string[] ViewContentLines)
        {
            AlternateView.ContentStream = ConcatenateStringArray(ViewContentLines);
        }

        private void ParseAttachment(ref EMailAttachment Attachment, string[] AttachmentLines)
        {
            int headerEndLine = FindHeaderEnd(AttachmentLines);
            ParseAttachmentHeaders(ref Attachment, GetHeaderLines(AttachmentLines, headerEndLine));
            ParseAttachmentContent(ref Attachment, GetBodyLines(AttachmentLines, headerEndLine));
        }
        private void ParseAttachmentHeaders(ref EMailAttachment Attachment, string[] AttachmentHeaderLines)
        {
            Regex validHeaderLineRegEx = new Regex(VALIDHEADERPATTERN);
            Match validHeaderLineMatch;
            ArrayList linkedRes = new ArrayList();

            string[] fixedMessageHeaderLines = SplitMultiValueLines(AttachmentHeaderLines);
            bool continuesFromPrevious = false;
            string lastHeaderProcessed = string.Empty;
            string headerName = string.Empty;
            string headerValue = string.Empty;

            foreach (string line in fixedMessageHeaderLines)
            {
                continuesFromPrevious = (line.StartsWith("\t") || line.StartsWith(" "));
                validHeaderLineMatch = validHeaderLineRegEx.Match(line);

                if (validHeaderLineMatch.Groups.Count < 3)
                {
                    continue;
                }

                if (continuesFromPrevious && CheckValidDateString(line))
                {
                    headerName = lastHeaderProcessed;
                    headerValue = line.Trim();
                }
                else
                {
                    headerName = validHeaderLineMatch.Groups[1].Value.Trim().ToLower();
                    headerValue = validHeaderLineMatch.Groups[2].Value.Trim();
                }

                switch (headerName)
                {
                    case "content-type":
                        Attachment.ContentType = GetCleanValue(headerValue);
                        break;
                    case "content-transfer-encoding":
                        Attachment.ContentTransferEncoding = GetCleanValue(headerValue);
                        break;
                    case "content-disposition":
                        Attachment.ContentDisposition = GetCleanValue(headerValue);
                        break;
                    case "name-encoding":
                        Attachment.NameEncoding = GetCleanValue(headerValue);
                        break;
                    case "ID":
                        Attachment.Id = GetCleanValue(headerValue);
                        break;
                    case "content-description":
                        Attachment.ContentDescription = GetCleanValue(headerValue);
                        break;
                    case "filename":
                        Attachment.FileName = GetCleanValue(headerValue);
                        break;
                    case "name":
                        Attachment.Name = GetCleanValue(headerValue);
                        break;
                }
            }
        }
        private void ParseAttachmentContent(ref EMailAttachment Attachment, string[] AttachmentContentLines)
        {
            Attachment.ContentStream = ConcatenateStringArray(AttachmentContentLines);
        }

        private void ParseNestedContent(string[] NestedCollectionLines, ref EMailBodyAlternateView[] Views, ref EMailAttachment[] Attachments)
        {
            string boundary = string.Empty;
            int[] boundariesPos = null;
            string[] nestedContent = null;
            string messageLine = string.Empty;

            int headerEndLine = FindHeaderEnd(NestedCollectionLines);
            string contentType = ParseNestedContentHeaders(GetHeaderLines(NestedCollectionLines, headerEndLine), out boundary);

            if (boundary != null && boundary != string.Empty)
            {
                boundariesPos = FindBoundariesPositions(NestedCollectionLines, boundary, headerEndLine);
            }

            EMailBodyAlternateView[] parsedViews = null;
            EMailAttachment[] parsedAttachments = null;

            switch (contentType)
            {
                case "multipart/alternative":
                    ParseMultipartAlternativeViews(GetBodyLines(NestedCollectionLines, headerEndLine), boundary, ref parsedViews, ref parsedAttachments);
                    break;
                case "multipart/mixed":
                case "multipart/related":
                    for (int i = 0; i < boundariesPos.Length - 1; )
                    {
                        nestedContent = GetStringArrayPart(GetBodyLines(NestedCollectionLines, headerEndLine), boundariesPos[i], boundariesPos[++i]);
                        ParseNestedContent(nestedContent, ref Views, ref Attachments);
                    }

                    break;

                case "text/plain":
                case "text/html":
                case "text/htm":
                case "":
                    parsedViews = new EMailBodyAlternateView[] { new EMailBodyAlternateView() };
                    ParseAlternativeView(ref parsedViews[0], NestedCollectionLines);
                    break;

                default:
                    parsedAttachments = new EMailAttachment[] { new EMailAttachment() };
                    ParseAttachment(ref parsedAttachments[0], NestedCollectionLines);
                    break;
            }

            if (Views == null) { Views = new EMailBodyAlternateView[0]; }
            if (Attachments == null) { Attachments = new EMailAttachment[0]; }
            if (parsedViews == null) { parsedViews = new EMailBodyAlternateView[0]; }
            if (parsedAttachments == null) { parsedAttachments = new EMailAttachment[0]; }

            EMailBodyAlternateView[] newViewsCollection = new EMailBodyAlternateView[Views.Length + parsedViews.Length];
            Array.Copy(Views, newViewsCollection, Views.Length);
            Array.Copy(parsedViews, 0, newViewsCollection, Views.Length, parsedViews.Length);

            EMailAttachment[] newAttachmentsCollection = new EMailAttachment[Attachments.Length + parsedAttachments.Length];
            Array.Copy(Attachments, newAttachmentsCollection, Attachments.Length);
            Array.Copy(parsedAttachments, 0, newAttachmentsCollection, Attachments.Length, parsedAttachments.Length);

            Views = newViewsCollection;
            Attachments = newAttachmentsCollection;

            return;
        }
        private string ParseNestedContentHeaders(string[] HeaderLines, out string MessageBoundary)
        {
            string contentType = string.Empty;
            MessageBoundary = string.Empty;

            Regex validHeaderLineRegEx = new Regex(VALIDHEADERPATTERN);
            Match validHeaderLineMatch;

            string[] fixedMessageHeaderLines = SplitMultiValueLines(HeaderLines);

            foreach (string line in fixedMessageHeaderLines)
            {
                validHeaderLineMatch = validHeaderLineRegEx.Match(line);

                string headerName = validHeaderLineMatch.Groups[1].Value.Trim().ToLower();
                string headerValue = validHeaderLineMatch.Groups[2].Value.Trim();

                switch (headerName)
                {
                    case "content-type":
                        contentType = GetCleanValue(headerValue);
                        break;
                    case "content-transfer-encoding":
                        //OriginalEmail.ContentTransferEncoding = GetCleanValue(headerValue);
                        break;
                    case "boundary":
                        string cleanBoundary = GetCleanValue(headerValue);
                        MessageBoundary = string.Format("--{0}", cleanBoundary);
                        break;
                }
            }
            return contentType;
        }

        private void ParseMultipartAlternativeViews(string[] ViewsCollectionLines, string Boundary, ref EMailBodyAlternateView[] Views, ref EMailAttachment[] Attachments)
        {
            int[] BoundariesPos = FindBoundariesPositions(ViewsCollectionLines, Boundary, 0);

            if (BoundariesPos != null && BoundariesPos.Length > 2)
            {
                ParseMultipartAlternativeViews(ViewsCollectionLines, BoundariesPos, ref Views, ref Attachments);
            }
            return;
        }

        private void ParseMultipartAlternativeViews(string[] ViewsCollectionLines, int[] BoundaryPositions, ref EMailBodyAlternateView[] Views, ref EMailAttachment[] Attachments)
        {
            EMailBodyAlternateView[] parsedViews = null;
            EMailAttachment[] parsedAttachments = null;

            if (BoundaryPositions != null && BoundaryPositions.Length > 2)
            {
                for (int i = 0; i < BoundaryPositions.Length - 1; )
                {
                    string[] viewLines = GetStringArrayPart(ViewsCollectionLines, BoundaryPositions[i] + 1, BoundaryPositions[++i] - 1);
                    ParseNestedContent(viewLines, ref parsedViews, ref parsedAttachments);
                }
            }

            if (Views == null) { Views = new EMailBodyAlternateView[0]; }
            if (Attachments == null) { Attachments = new EMailAttachment[0]; }
            if (parsedViews == null) { parsedViews = new EMailBodyAlternateView[0]; }
            if (parsedAttachments == null) { parsedAttachments = new EMailAttachment[0]; }

            EMailBodyAlternateView[] newViewsCollection = new EMailBodyAlternateView[Views.Length + parsedViews.Length];
            Array.Copy(Views, newViewsCollection, Views.Length);
            Array.Copy(parsedViews, 0, newViewsCollection, Views.Length, parsedViews.Length);

            EMailAttachment[] newAttachmentsCollection = new EMailAttachment[Attachments.Length + parsedAttachments.Length];
            Array.Copy(Attachments, newAttachmentsCollection, Attachments.Length);
            Array.Copy(parsedAttachments, 0, newAttachmentsCollection, Attachments.Length, parsedAttachments.Length);

            Views = newViewsCollection;
            Attachments = newAttachmentsCollection;

            return;
        }

        private int FindHeaderEnd(string[] FullContent)
        {
            int result = -1;
            if (FullContent.Length < 3)
            {
                return result;
            }

            int currentLine = -1;
            if (FullContent[0] != string.Empty)
            {
                currentLine = 0;
            }
            else
            {
                currentLine = 1;
            }

            while (FullContent[currentLine].Trim() != string.Empty)
            {
                ++currentLine;
            }

            result = currentLine;

            return result;
        }

        private int[] FindBoundariesPositions(string[] MessageLines, string Boundary, int HeaderEndPosition)
        {
            int[] result = null;

            if (Boundary != null && Boundary != string.Empty)
            {
                ArrayList linesNumbers = new ArrayList();

                for (int i = HeaderEndPosition; i < MessageLines.Length; ++i)
                {
                    if (MessageLines[i].Trim().Equals(Boundary) || MessageLines[i].Trim().Equals(string.Format("{0}--", Boundary)))
                    {
                        linesNumbers.Add(i - HeaderEndPosition - 1);
                    }
                }
                result = new int[linesNumbers.Count];
                linesNumbers.CopyTo(result);
            }

            return result;
        }

        private string[] GetHeaderLines(string[] FullContent, int HeaderEndLineNumber)
        {
            string[] result = new string[HeaderEndLineNumber];
            Array.Copy(FullContent, result, HeaderEndLineNumber);
            return result;
        }

        private string[] GetBodyLines(string[] FullContent, int HeaderEndLineNumber)
        {
            string[] result = new string[FullContent.Length - HeaderEndLineNumber];
            Array.Copy(FullContent, HeaderEndLineNumber, result, 0, FullContent.Length - HeaderEndLineNumber);
            return result;
        }

        private string[] GetStringArrayPart(string[] StringArray, int StartIndex, int EndIndex)
        {
            string[] result = null;
            if (StringArray == null || StartIndex < 0 || EndIndex > StringArray.Length - 1 || StringArray.Length < 1)
            {
                return result;
            }

            result = new string[EndIndex - StartIndex + 1];
            for (int i = StartIndex, j = 0; i <= EndIndex; ++i, ++j)
            {
                result[j] = StringArray[i];
            }

            return result;
        }

        private string GetCleanValue(string DirtyValue)
        {
            string result;

            result = DirtyValue.Replace("\"", string.Empty);
            result = result.Replace("\\", string.Empty);
            result = result.Replace(";", string.Empty);
            result = result.Trim();

            return result;
        }

        private string ConcatenateStringArray(string[] BodyPartLines)
        {
            return ConcatenateStringArray(BodyPartLines, false);
        }
        private string ConcatenateStringArray(string[] BodyPartLines, bool KeepTextOnly)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in BodyPartLines)
            {
                if (KeepTextOnly)
                {
                    if (line != string.Empty)
                    {
                        sb.Append(line);
                    }
                }
                else
                {
                    sb.AppendFormat("{0}{1}", line, NEWLINE);
                }
            }
            return sb.ToString();
        }

        private EMailAddress[] ParseEmailAddressTag(string emailHeaderTag)
        {
            EMailAddress[] result;
            char[] sepatators = ",;".ToCharArray();
            string[] emails = emailHeaderTag.Split(sepatators);
            ArrayList list = new ArrayList();
            foreach (string emailTag in emails)
            {
                EMailAddress address = ParseEmailAddress(emailTag.Trim());
                if (address.DisplayName != null && address.Host != null)
                { list.Add(address); }
            }
            result = new EMailAddress[list.Count];
            list.CopyTo(result);
            return result;
        }

        private EMailAddress ParseEmailAddress(string emailAddressTag)
        {
            EMailAddress result = null;
            string[] parts = new string[2];
            if (EMailAddress.ValidateAddress(emailAddressTag))
            {
                parts[0] = emailAddressTag;
                parts[1] = string.Empty;
            }
            else
            {
                Regex regEx = new Regex(@"<.*>$");
                Match match = regEx.Match(emailAddressTag);
                if (match.Success)
                {
                    parts[0] = match.Value;
                    parts[0] = parts[0].Substring(1, parts[0].Length - 2);
                    parts[1] = emailAddressTag.Remove(match.Index).Trim();
                }
                else
                {
                    parts[0] = parts[1] = emailAddressTag;
                }
            }
            result = new EMailAddress(parts[0], parts[1]);
            return result;
        }

        private string RemoveInvalidChars(string Input)
        {
            string result = string.Empty;

            Regex rg = new Regex("&#x([[0-9][A-F]]{3});", RegexOptions.Multiline);
            MatchCollection founds = rg.Matches(Input);

            result = rg.Replace(Input, "&#x040;");
            return result;
        }

        private string[] SplitMultiValueLines(string[] Lines)
        {
            string[] result = new string[Lines.Length];

            string currentLine;
            int patchedCount = 0;

            for (int i = 0; i < Lines.Length; ++i)
            {
                currentLine = Lines[i];
                if (currentLine.StartsWith("Subject:", StringComparison.InvariantCultureIgnoreCase))
                {
                    result[patchedCount] = currentLine;
                    patchedCount++;
                    continue;
                }

                int idxSemiColon = currentLine.Trim().IndexOf(';');
                if (idxSemiColon < 0 || idxSemiColon == currentLine.Trim().Length - 1)
                {
                    result[patchedCount] = currentLine;
                    patchedCount++;
                    continue;
                }

                if (currentLine.Contains(";"))
                {
                    string[] patchLines = currentLine.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] patchedResult = new string[result.Length + patchLines.Length - 1];

                    for (int j = 0; j < patchedCount; j++)
                    {
                        patchedResult[j] = result[j];
                    }
                    for (int k = 0; k < patchLines.Length; k++)
                    {
                        patchedResult[patchedCount + k] = patchLines[k];

                        if (k < patchLines.Length - 1)
                        {
                            patchedResult[patchedCount + k] += ";";
                        }
                    }
                    patchedCount += patchLines.Length;
                    result = patchedResult;
                }
                else
                {
                    result[patchedCount] = currentLine;
                    patchedCount++;
                }
            }
            return result;
        }

        private string GetValidDateString(string Input)
        {
            Regex rx = new Regex(@"^(([a-zA-Z]{3},\s){0,1}\d{0,2}\s{0,1}[a-zA-Z]{3}\s\d{4}\s\d{2}:\d{2}:\d{2}\s([-+]\d{4}){0,1})");
            Match match = rx.Match(Input.Trim());
            return match.Groups[1].ToString();
        }

        private bool CheckValidDateString(string Input)
        {
            return !(string.Empty == GetValidDateString(Input));
        }
    }
}