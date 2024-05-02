using System;
using System.Runtime.Serialization;

namespace BLL.TableParameters
{
    [Serializable]
    [DataContract]
    public class DataTablesOrder
    {
        [DataMember(Name = "column")]
        public int Column { get; set; }

        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}