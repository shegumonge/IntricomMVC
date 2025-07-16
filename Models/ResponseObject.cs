using System;
using System.Collections.Generic;
using System.Text;

namespace IntricomMVC.Models
{
    public class ResponseObject
    {
        #region Atributos

        private Boolean _success;

        private String _message;

        private Object _data;

        #endregion

        #region Constructores

        public ResponseObject(Boolean success)
        {
            _success = success;
        }

        public ResponseObject(Boolean success, String message, Object data = null)
        {
            _success = success;
            _message = message;
            _data = data;
        }

        #endregion

        #region Propiedades

        public Boolean success
        {
            get
            {
                return _success;
            }
            set
            {
                _success = value;
            }
        }

        public String message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public Object data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        #endregion
    }
}
