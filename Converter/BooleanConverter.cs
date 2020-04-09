using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace SiProd.Utilities.Tools
{
    public class BooleanBindingHelper : IBindableComponent
    {
        private bool orignal;

        private BindingContext propertyManager;
        private ControlBindingsCollection controlBindingsCollection;
        private event EventHandler disposed;

        private BooleanBindingHelperSite site;

        public bool Original
        {
            get { return orignal; }
            set { orignal = value; }
        }
        public bool Opposite
        {
            get { return !orignal; }
        }

        public void SetValueToDeny(bool Value)
        {
            orignal = Value;
        }
        public BooleanBindingHelper()
        {
            controlBindingsCollection = new ControlBindingsCollection(this);
        }

        #region IBindableComponent Members

        BindingContext IBindableComponent.BindingContext
        {
            get
            {
                return propertyManager;
            }
            set
            {
                propertyManager = value;
            }
        }

        ControlBindingsCollection IBindableComponent.DataBindings
        {
            get { return controlBindingsCollection; }
        }

        #endregion

        #region IComponent Members

        event EventHandler System.ComponentModel.IComponent.Disposed
        {
            add { disposed += value; }
            remove { disposed -= value; }
        }

        System.ComponentModel.ISite System.ComponentModel.IComponent.Site
        {
            get
            {
                return site;
            }
            set
            {
                site = (BooleanBindingHelperSite)value;
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (disposed != null)
            {
                disposed(this, new EventArgs());
            }
        }

        #endregion
    }

    public class BooleanBindingHelperSite : System.ComponentModel.ISite
    {
        private System.ComponentModel.IComponent component;
        private System.ComponentModel.IContainer container;
        private bool designMode;
        private string name;

        public BooleanBindingHelperSite(System.ComponentModel.IContainer Container, System.ComponentModel.IComponent Component)
        {
            container = Container;
            component = Component;
            designMode = false;
            name = null;
        }


        #region ISite Members

        System.ComponentModel.IComponent System.ComponentModel.ISite.Component
        {
            get { return component; }
        }

        System.ComponentModel.IContainer System.ComponentModel.ISite.Container
        {
            get { return container; }
        }

        bool System.ComponentModel.ISite.DesignMode
        {
            get { return designMode; }
        }

        string System.ComponentModel.ISite.Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        #endregion

        #region IServiceProvider Members

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
