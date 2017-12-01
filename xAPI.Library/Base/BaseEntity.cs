using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Globalization;
using Microsoft.SqlServer.Server;
using System.Data;

namespace xAPI.Library.Base
{
    /// <summary>
    ///Esta clase Padre tiene que Heredar de todas las clase de la Capa Entidad.
    /// </summary>
    [Serializable]
    public class BaseEntity
    {
        #region "Atributos"
        public Int32 ID { get; set; }
        public Int64 LegacyId { get; set; }
        String name;
        public Int16 AllowSxm { get; set; }

        public String Name
        {

            get
            {
                name = name ?? String.Empty;
                return name;
            }
            set
            {
                name = value;
            }
        }

        String description;
        public String Description
        {

            get
            {
                description = description ?? String.Empty;
                return description;
            }
            set
            {
                description = value;
            }
        }

        public Int32 Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Int32 Createdby { get; set; }
        public Int32 Updatedby { get; set; }
        public CultureInfo Culture { get; set; }
        public String IdDele { get; set; }
        public String Type { get; set; } // solo aplica para prestamos y adelantos por estar  en una sola lista(dos tablas)



        [XmlIgnore()]
        public List<ListError> Errors { get; set; }
        public Int16 Action { get; set; }
        public String SerializationData { get; set; }
        public String Location { get; set; }
        public String Latitude { get; set; }
        public String Longitude { get; set; }
        #endregion

        #region "Constructores"

        public BaseEntity()
        {
            this.Errors = new List<ListError>();
        }
        /*
        public BaseEntity(int id, string name, string state, DateTime creationdate, DateTime modifieddate)
        {
            this.Id = id;
            this.Name = name;
            this.State = state;
            this.CreationDate = creationdate;
            this.ModifiedDate = modifieddate;
        }*/

        #endregion
        [Serializable]
        public class ListError
        {
            public Exception Error { get; set; }
            public String MessageClient { get; set; }

            public ListError()
            {

            }
            public ListError(Exception error, String message)
            {
                this.Error = error;
                this.MessageClient = message;

            }

        }

      


    }
}
