using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Anything_wpf_main_.cls
{
   public class HotKeyItem: ICloneable
    {

        public enum HotKeyParentType
        {
            Item=0,
            Pluygin=1
        }
        public HotKeyItem(object iParent, System.Windows.Forms.Keys key,uint modifiers,int ID,HotKeyParentType Type=HotKeyParentType.Item)
        {
            this.iParent = iParent;
            this.KeyValue_ = key;
            this.ModifiersValue_ = modifiers;
            this.ID_ = ID;
            this.ParentType = Type;
        }


        #region Attribute

        private System.Windows.Forms.Keys KeyValue_ = System.Windows.Forms.Keys.None;
        private uint ModifiersValue_ = 0;
        private int ID_ = 0x0000;
        private HotKeyParentType Parenttype = HotKeyParentType.Item;
        private object iParent = "";

        #endregion

        #region AttributePacking

        public System.Windows.Forms.Keys KeyValue
        {
            get
            {
                return KeyValue_;
            }

            set
            {
                KeyValue_ = value;
            }
        }

        public uint ModifiersValue
        {
            get
            {
                return ModifiersValue_;
            }

            set
            {
                ModifiersValue_ = value;
            }
        }

        public int ID
        {
            get
            {
                return ID_;
            }

            set
            {
                ID_ = value;
            }
        }

        public HotKeyParentType ParentType
        {
            get
            {
                return Parenttype;
            }

            set
            {
                Parenttype = value;
            }
        }

        public object IParent
        {
            get
            {
                return iParent;
            }

            set
            {
                iParent = value;
            }
        }

        public object Clone()
        {
            return new HotKeyItem(this.iParent, this.KeyValue, this.ModifiersValue,this.ID,this.ParentType);
        }

        #endregion
    }
}
