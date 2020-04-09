using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.FileTags.Exif.MakerNotes
{
    enum Maker
    {
        UNKNOWN = 0,
        Agfa = 1,
        Canon = 2,
        Casio = 3,
        Epson = 4,
        Fujifilm = 5,
        Konica = 6,
        Minolta = Konica,
        Kyocera = 7,
        Contax = Kyocera,
        Nikon = 8,
        Olympus = 9,
        Panasonic = 10,
        Pentax = 11,
        Asahi = Pentax,
        Ricoh = 12,
        Sony = 13,
    }
}
