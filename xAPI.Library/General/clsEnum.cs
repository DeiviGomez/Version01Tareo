using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xAPI.Library.Security;

namespace xAPI.Library.General
{
    public static class clsEnum
    {

        public static List<object> LoadEnumValues(Type _enum,Boolean Encrypt = false)
        {
            List<object> lst = new List<object>();
            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            for (int i = 1; i < fi.Length; i++)
            {
                StringMessageAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringMessageAttribute), false) as StringMessageAttribute[];

                lst.Add(new
                {
                    name = attrs.Length > 0 ? attrs[0].StringMessage : "",
                    id = Encrypt ? clsEncryption.Encrypt(Convert.ToInt32(values.GetValue(i - 1)).ToString()) : Convert.ToInt32(values.GetValue(i - 1)).ToString()
                });
            }

            return lst;
        }


        public static List<object> LoadEnumValues2(Type _enum, Boolean Encrypt = false)
        {
            List<object> lst = new List<object>();
            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            for (int i = 1; i < fi.Length; i++)
            {
                StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

                lst.Add(new
                {
                    name = attrs.Length > 0 ? attrs[0].StringValue : "",
                    id = Encrypt ? clsEncryption.Encrypt(Convert.ToInt32(values.GetValue(i - 1)).ToString()) : Convert.ToInt32(values.GetValue(i - 1)).ToString()
                });
            }

            return lst;
        }


        public static String SubGetStringValue(this Type value,Int32 id)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this typeAddress
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];


            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
        public static String GetValueById(Type _enum, Int32 attr)
        {

            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            String Name = "";
            for (int i = 1; i < fi.Length; i++)
            {

                if (Convert.ToInt32(values.GetValue(i - 1)) == attr)
                {

                    StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                    {
                        Name = attrs[0].StringValue;
                    }
                    //Name = values.GetValue(i - 1).ToString();
                    break;
                }
            }

            return Name;
        }
        public static String GetValueById2(Type _enum, Int32 attr)
        {

            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            String Name = "";
            String namecountry = "";
            for (int i = 0; i < values.Length; i++)
            {
                if (Convert.ToInt32(values.GetValue(i)) == attr)
                {
                    namecountry = values.GetValue(i).ToString();
                    break;
                }
            }
            for (int i = 1; i < fi.Length; i++)
            {

                if (fi[i].Name == namecountry)
                {

                    StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                    {
                        Name = attrs[0].StringValue;
                    }
                    //Name = values.GetValue(i - 1).ToString();
                    break;
                }
            }

            return Name;
        }
        public static String GetMessageById(Type _enum, Int32 attr)
        {

            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();
            String Name = "";
            for (int i = 1; i < fi.Length; i++)
            {

                if (Convert.ToInt32(values.GetValue(i - 1)) == attr)
                {

                    StringMessageAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringMessageAttribute), false) as StringMessageAttribute[];
                    if (attrs.Length > 0)
                    {
                        Name = attrs[0].StringMessage;
                    }
                    //Name = values.GetValue(i - 1).ToString();
                    break;
                }
            }

            return Name;
        }
        public static String GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].StringValue;
            }
            return output;
        }
        public static String GetStringValue2(Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].StringValue;
            }
            return output;
        }
        public static Int32 GetValueByStringAttr(Type _enum, String attr)
        {
            Int32 ret = 0;
            Array values = Enum.GetValues(_enum);
            FieldInfo[] fi = _enum.GetFields();

            for (int i = 1; i < fi.Length; i++)
            {
                StringValueAttribute[] attrs = fi[i].GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs[0].StringValue == attr)
                {
                    ret = Convert.ToInt32(values.GetValue(i-1));
                }
            }

            return ret;
        }
        public static String GetStringValue(Type _enum, Int32 IdEnum){

            String stringValue = String.Empty;
            Array arrayValues = Enum.GetValues(_enum);
            
            string[] names = System.Enum.GetNames(_enum);
            for(int i = 0;i< arrayValues.Length;i++){
                if ((Int32)arrayValues.GetValue(i) == IdEnum)
                {
                    Enum enumObj = (Enum)Enum.Parse(_enum, names[i]);
                    stringValue = enumObj.GetStringValue();
                    break;
                }
            }
            return stringValue;
        }
        public static String GetMessageValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringMessageAttribute[] attrs = fi.GetCustomAttributes(typeof(StringMessageAttribute), false) as StringMessageAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].StringMessage;
            }
            return output;
        }
    }

    #region Extension of Attribute
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }
    }
    public class StringMessageAttribute : Attribute
    {
        public string StringMessage { get; protected set; }
        public StringMessageAttribute(string value)
        {
            this.StringMessage = value;
        }
    }
    #endregion

   
    public enum EnumEntity 
    {
        Individual = 1,
        Business = 2
    }

    public enum EnumAccion
    {
        Insert = 1,
        Edit = 2,
        Delete = 3,
        Ninguno = 0
    }

    public enum EnumTypeLoans
    {

        [StringValue("Préstamo")]
        [StringMessage("green")]
        Loan = 0,//pasar a ingles
       
        [StringValue("Adelanto")]
        [StringMessage("Orange")]
        Advancement = 1,//pasar a ingles,

        [StringValue("1")] // prestamo
        type1 = 2,//pasar a ingles,

        [StringValue("2")] // adelanto
        type2 = 3,//pasar a ingles,

        [StringValue("____")] // adelanto
        limitdate = 4//pasar a ingles,
        
    }
    public enum EnumTypeComision
    {
        [StringValue("1")]
        tipo1 = 1,

        [StringValue("2")]
        tipo2 = 2,
        
    }
    public enum EnumTypeSeguro
    {
        [StringValue("1")]
        tipo1 = 1,

        [StringValue("2")]
        tipo2 = 2,

    }
    //condiciones "principal" y "secundario" para plantilla Vacaciones
    public enum EnumConditionPlantVaca
    {

        [StringValue("Principal")]
        [StringMessage("green")]
        Principal = 0,//pasar a ingles

        [StringValue("Secundario")]
        [StringMessage("Orange")]
        Secondary = 1//pasar a ingles,

    }

    //condiciones "principal" y "secundario" para plantilla memorandum
    public enum EnumConditionPlantMemo
    {

        [StringValue("Principal")]
        [StringMessage("green")]
        Principal=0,//pasar a ingles

        [StringValue("Secundario")]
        [StringMessage("Orange")]
        Secondary=1//pasar a ingles,

    }
    /*Condiciones "principal" y "Secundario" para los horarios */
    public enum EnumCondicionHorario
    {
        [StringValue("Principal")]
        [StringMessage("green")]
        Principal=0,

        [StringValue("Secundario")]
        [StringMessage("orange")]
        Secundario = 1,
    }

    public enum EnumCondicionVacaciones
    {
        [StringValue("Por Programar")]
        [StringMessage("Red")]
        proprogramar = 0,

        [StringValue("Programado")]
        [StringMessage("Blue")]
        programado = 1,

        [StringValue("Vacaciones")]
        [StringMessage("green")]
        vacaciones = 2,

        [StringValue("Vacaciones Truncas")]
        [StringMessage("Orange")]
        truncas = 3,
    }

    public enum EnumCondicionSalidaVacaciones
    {
        [StringValue("Salió de Vacaciones")]
        [StringMessage("Blue")]
        salio = 0,

        [StringValue("No salió de vacaciones")]
        [StringMessage("Red")]
        nosalio = 1,
        [StringValue("Tiene Vacaciones Truncas")]
        [StringMessage("Orange")]
        truncas = 3,
    }
    //condiciones "positivo" y "negativo" para la asignacion del memorandum al trabajador
    public enum EnumConditionTipoMemorandum
    {

        [StringValue("Positivo")]
        [StringMessage("green")]
        Positivo = 1,//pasar a ingles //este 0 no tiene nada q ver con la condicion..solo indica q es el primer campo

        [StringValue("Negativo")]
        [StringMessage("Red")]
        Negativo = 0//pasar a ingles //este 1 no tiene nada q ver con la condicion..solo indica q es el primer campo

    }
    public enum enumConditionEstadoContrato
    {
        [StringValue("Contrato Actual")]
        [StringMessage("green")]
        Actual = 1,//pasar a ingles //este 0 no tiene nada q ver con la condicion..solo indica q es el primer campo

        [StringValue("Contrato Archivado")]
        [StringMessage("Red")]
        Archivado = 2//pasar a ingles //este 1 no tiene nada q ver con la condicion..solo indica q es el primer campo

    }

    public enum EnumContactMethod
    {
        Email = 1,
        Phone = 2
    }
    public enum EnumEnrollmentSponsorUrl
    {
        Username = 1,
        PromoterId = 2,
        Both = 3
    }

    public enum EnumFnClient
    {
        [StringValue("fn_buildtable();")]
        buildtable = 0,
        [StringValue("fn_filltable({0});")]
        filltable = 1,
        [StringValue("{0}({1});")]
        generic = 2
    }
    public enum EnumActionF4W
    {
        [StringValue("Try again")]
        TRYAGAIN = 0,
        [StringValue("Successful,Flag")]
        SUCCESSFUL = 1,
        [StringValue("7 Days in")]
        DAYSIN = 2,
        [StringValue("Un-Flag")]
        UnFlag = 3
    }
    public enum EnumCardType
    {
        [StringValue("Visa")]
        Visa = 1,
        [StringValue("MasterCard")]
        MasterCard = 2,
        [StringValue("American Express")]
        AmericanExpress = 3,
        [StringValue("Discover")]
        Discover = 4,
        [StringValue("Diners")]
        Diners = 5

    }
    public enum EnumStatusParty
    {

        [StringValue("Opened")]
        Opened = 0,
        [StringValue("Closed")]
        Closed = 1

    }
    public enum EstadoEmpresa
    {
        [StringValue("Activo")]
        Activo = 1,
        [StringValue("Desactivado")]
        Desactivado = 0
    }
    public enum RadioDLSS 
    {
        [StringValue("Si")]
        Activo = 1,
        [StringValue("No")]
        Desactivado = 0
    }
    public enum Reset
    {
        [StringValue("1")]
        First = 1,
    }
    public enum EnumPartyScheduleStatus
    {
        [StringValue("Pending")]
        Pending = 0,
        [StringValue("Approved")]
        Approved = 1,
        [StringValue("Not Approved")]
        NotApproved = 2
    }
    public enum EnumGender
    {
        [StringValue("Masculino")]
        male = 1,
        [StringValue("Femenino")]
        female = 2
    }

    public enum EnumRegimen    {
        [StringValue("Private")]
        privado = 1,
        [StringValue("Public")]
        publico = 2
    }
    public enum Usuariov1
    {
        [StringValue("change")]
        change = 1,
    }
    public enum EnumDepartamentos
    {
        [StringValue("---Seleccione Opción---")]
        depa0 = 0,
        [StringValue("AMAZONAS")]
        depa1 = 1,
        [StringValue("ANCASH")]
        depa2 = 2,
        [StringValue("APURIMAC")]
        depa3 = 3,
        [StringValue("AREQUIPA")]
        depa4 = 4,
        [StringValue("AYACUCHO")]
        depa5 = 5,
        [StringValue("CAJAMARCA")]
        depa6 = 6,
        [StringValue("CUSCO")]
        depa7 = 7,
        [StringValue("HUANCAVELICA")]
        depa8 = 8,
        [StringValue("HUANUCO")]
        depa9 = 9,
        [StringValue("ICA")]
        depa10 = 10,
        [StringValue("JUNIN")]
        depa11 = 11,
        [StringValue("LA LIBERTAD")]
        depa12 = 12,
        [StringValue("LAMBAYEQUE")]
        depa13 = 13,
        [StringValue("LIMA")]
        depa14 = 14,
        [StringValue("LORETO")]
        depa15 = 15,
        [StringValue("MADRE DE DIOS")]
        depa16 = 16,
        [StringValue("MOQUEGUA")]
        depa17 = 17,
        [StringValue("PASCO")]
        depa18 = 18,
        [StringValue("PIURA")]
        depa19 = 19,
        [StringValue("PUNO")]
        depa20 = 20,
        [StringValue("SAN MARTIN")]
        depa21 = 21,
        [StringValue("TACNA")]
        depa22 = 22,
        [StringValue("TUMBES")]
        depa23 = 23,
        [StringValue("PROV. CONST. DEL CALLAO")]
        depa24 = 24,
        [StringValue("UCAYALI")]
        depa25 = 25
    }

    public enum EnumProvincias
    {
        [StringValue("---Seleccione Opción---")]
        prov0 = 0,
        [StringValue("CHACHAPOYAS")]
        prov1 = 1,
        [StringValue("BAGUA")]
        prov2 = 2,
        [StringValue("BONGARA")]
        prov3 = 3,
        [StringValue("LUYA")]
        prov4 = 4,
        [StringValue("RODRIGUEZ DE MENDOZA")]
        prov5 = 5,
        [StringValue("CONDORCANQUI")]
        prov6 = 6,
        [StringValue("UTCUBAMBA")]
        prov7 = 7,
        [StringValue("HUARAZ")]
        prov8 = 8,
        [StringValue("AIJA")]
        prov9 = 9,
        [StringValue("BOLOGNESI")]
        prov10 = 10,
        [StringValue("CARHUAZ")]
        prov11 = 11,
        [StringValue("CASMA")]
        prov12 = 12,
        [StringValue("CORONGO")]
        prov13 = 13,
        [StringValue("HUAYLAS")]
        prov14 = 14,
        [StringValue("HUARI")]
        prov15 = 15,
        [StringValue("MARISCAL LUZURIAGA")]
        prov16 = 16,
        [StringValue("PALLASCA")]
        prov17 = 17,
        [StringValue("POMABAMBA")]
        prov18 = 18,
        [StringValue("RECUAY")]
        prov19 = 19,
        [StringValue("SANTA")]
        prov20 = 20,
        [StringValue("SIHUAS")]
        prov21 = 21,
        [StringValue("YUNGAY")]
        prov22 = 22,
        [StringValue("ANTONIO RAIMONDI")]
        prov23 = 23,
        [StringValue("CARLOS FERMIN FITZCARRALD")]
        prov24 = 24,
        [StringValue("ASUNCION")]
        prov25 = 25,
        [StringValue("HUARMEY")]
        prov26 = 26,
        [StringValue("OCROS")]
        prov27 = 27,
        [StringValue("ABANCAY")]
        prov28 = 28,
        [StringValue("AYMARAES")]
        prov29 = 29,
        [StringValue("ANDAHUAYLAS")]
        prov30 = 30,
        [StringValue("ANTABAMBA")]
        prov31 = 31,
        [StringValue("COTABAMBAS")]
        prov32 = 32,
        [StringValue("GRAU")]
        prov33 = 33,
        [StringValue("CHINCHEROS")]
        prov34 = 34,
        [StringValue("AREQUIPA")]
        prov35 = 35,
        [StringValue("CAYLLOMA")]
        prov36 = 36,
        [StringValue("CAMANA")]
        prov37 = 37,
        [StringValue("CARAVELI")]
        prov38 = 38,
        [StringValue("CASTILLA")]
        prov39 = 39,
        [StringValue("CONDESUYOS")]
        prov40 = 40,
        [StringValue("ISLAY")]
        prov41 = 41,
        [StringValue("LA UNION")]
        prov42 = 42,
        [StringValue("HUAMANGA")]
        prov43 = 43,
        [StringValue("CANGALLO")]
        prov44 = 44,
        [StringValue("HUANTA")]
        prov45 = 45,
        [StringValue("LA MAR")]
        prov46 = 46,
        [StringValue("LUCANAS")]
        prov47 = 47,
        [StringValue("PARINACOCHAS")]
        prov48 = 48,
        [StringValue("VICTOR FAJARDO")]
        prov49 = 49,
        [StringValue("HUANCA SANCOS")]
        prov50 = 50,
        [StringValue("VILCAS HUAMAN")]
        prov51 = 51,
        [StringValue("PAUCAR DEL SARA SARA")]
        prov52 = 52,
        [StringValue("SUCRE")]
        prov53 = 53,
        [StringValue("CAJAMARCA")]
        prov54 = 54,
        [StringValue("CAJABAMBA")]
        prov55 = 55,
        [StringValue("CELENDIN")]
        prov56 = 56,
        [StringValue("CONTUMAZA")]
        prov57 = 57,
        [StringValue("CUTERVO")]
        prov58 = 58,
        [StringValue("CHOTA")]
        prov59 = 59,
        [StringValue("HUALGAYOC")]
        prov60 = 60,
        [StringValue("JAEN")]
        prov61 = 61,
        [StringValue("SANTA CRUZ")]
        prov62 = 62,
        [StringValue("SAN MIGUEL")]
        prov63 = 63,
        [StringValue("SAN IGNACIO")]
        prov64 = 64,
        [StringValue("SAN MARCOS")]
        prov65 = 65,
        [StringValue("SAN PABLO")]
        prov66 = 66,
        [StringValue("CUSCO")]
        prov67 = 67,
        [StringValue("ACOMAYO")]
        prov68 = 68,
        [StringValue("ANTA")]
        prov69 = 69,
        [StringValue("CALCA")]
        prov70 = 70,
        [StringValue("CANAS")]
        prov71 = 71,
        [StringValue("CANCHIS")]
        prov72 = 72,
        [StringValue("CHUMBIVILCAS")]
        prov73 = 73,
        [StringValue("ESPINAR")]
        prov74 = 74,
        [StringValue("LA CONVENCION")]
        prov75 = 75,
        [StringValue("PARURO")]
        prov76 = 76,
        [StringValue("PAUCARTAMBO")]
        prov77 = 77,
        [StringValue("QUISPICANCHIS")]
        prov78 = 78,
        [StringValue("URUBAMBA")]
        prov79 = 79,
        [StringValue("HUANCAVELICA")]
        prov80 = 80,
        [StringValue("ACOBAMBA")]
        prov81 = 81,
        [StringValue("ANGARAES")]
        prov82 = 82,
        [StringValue("CASTROVIRREYNA")]
        prov83 = 83,
        [StringValue("TAYACAJA")]
        prov84 = 84,
        [StringValue("HUAYTARA")]
        prov85 = 85,
        [StringValue("CHURCAMPA")]
        prov86 = 86,
        [StringValue("HUANUCO")]
        prov87 = 87,
        [StringValue("AMBO")]
        prov88 = 88,
        [StringValue("DOS DE MAYO")]
        prov89 = 89,
        [StringValue("HUAMALIES")]
        prov90 = 90,
        [StringValue("MARANON")]
        prov91 = 91,
        [StringValue("LEONCIO PRADO")]
        prov92 = 92,
        [StringValue("PACHITEA")]
        prov93 = 93,
        [StringValue("PUERTO INCA")]
        prov94 = 94,
        [StringValue("HUACAYBAMBA")]
        prov95 = 95,
        [StringValue("LAURICOCHA")]
        prov96 = 96,
        [StringValue("YAROWILCA")]
        prov97 = 97,
        [StringValue("ICA")]
        prov98 = 98,
        [StringValue("CHINCHA")]
        prov99 = 99,
        [StringValue("NAZCA")]
        prov100 = 100,
        [StringValue("PISCO")]
        prov101 = 101,
        [StringValue("PALPA")]
        prov102 = 102,
        [StringValue("HUANCAYO")]
        prov103 = 103,
        [StringValue("CONCEPCION")]
        prov104 = 104,
        [StringValue("JAUJA")]
        prov105 = 105,
        [StringValue("JUNIN")]
        prov106 = 106,
        [StringValue("TARMA")]
        prov107 = 107,
        [StringValue("YAULI")]
        prov108 = 108,
        [StringValue("SATIPO")]
        prov109 = 109,
        [StringValue("CHANCHAMAYO")]
        prov110 = 110,
        [StringValue("CHUPACA")]
        prov111 = 111,
        [StringValue("TRUJILLO")]
        prov112 = 112,
        [StringValue("BOLIVAR")]
        prov113 = 113,
        [StringValue("SANCHEZ CARRION")]
        prov114 = 114,
        [StringValue("OTUZCO")]
        prov115 = 115,
        [StringValue("PACASMAYO")]
        prov116 = 116,
        [StringValue("PATAZ")]
        prov117 = 117,
        [StringValue("SANTIAGO DE CHUCO")]
        prov118 = 118,
        [StringValue("ASCOPE")]
        prov119 = 119,
        [StringValue("CHEPEN")]
        prov120 = 120,
        [StringValue("JULCAN")]
        prov121 = 121,
        [StringValue("GRAN CHIMU")]
        prov122 = 122,
        [StringValue("VIRU")]
        prov123 = 123,
        [StringValue("CHICLAYO")]
        prov124 = 124,
        [StringValue("FERRENAFE")]
        prov125 = 125,
        [StringValue("LAMBAYEQUE")]
        prov126 = 126,
        [StringValue("LIMA")]
        prov127 = 127,
        [StringValue("CAJATAMBO")]
        prov128 = 128,
        [StringValue("CANTA")]
        prov129 = 129,
        [StringValue("CANETE")]
        prov130 = 130,
        [StringValue("HUAURA")]
        prov131 = 131,
        [StringValue("HUAROCHIRI")]
        prov132 = 132,
        [StringValue("YAUYOS")]
        prov133 = 133,
        [StringValue("HUARAL")]
        prov134 = 134,
        [StringValue("BARRANCA")]
        prov135 = 135,
        [StringValue("OYON")]
        prov136 = 136,
        [StringValue("MAYNAS")]
        prov137 = 137,
        [StringValue("ALTO AMAZONAS")]
        prov138 = 138,
        [StringValue("LORETO")]
        prov139 = 139,
        [StringValue("REQUENA")]
        prov140 = 140,
        [StringValue("UCAYALI")]
        prov141 = 141,
        [StringValue("MARISCAL RAMON CASTILLA")]
        prov142 = 142,
        [StringValue("DATEM DEL MARAÑON")]
        prov143 = 143,
        [StringValue("TAMBOPATA")]
        prov144 = 144,
        [StringValue("MANU")]
        prov145 = 145,
        [StringValue("TAHUAMANU")]
        prov146 = 146,
        [StringValue("MARISCAL NIETO")]
        prov147 = 147,
        [StringValue("GENERAL SANCHEZ CERRO")]
        prov148 = 148,
        [StringValue("ILO")]
        prov149 = 149,
        [StringValue("PASCO")]
        prov150 = 150,
        [StringValue("DANIEL ALCIDES CARRION")]
        prov151 = 151,
        [StringValue("OXAPAMPA")]
        prov152 = 152,
        [StringValue("PIURA")]
        prov153 = 153,
        [StringValue("AYABACA")]
        prov154 = 154,
        [StringValue("HUANCABAMBA")]
        prov155 = 155,
        [StringValue("MORROPON")]
        prov156 = 156,
        [StringValue("PAITA")]
        prov157 = 157,
        [StringValue("SULLANA")]
        prov158 = 158,
        [StringValue("TALARA")]
        prov159 = 159,
        [StringValue("SECHURA")]
        prov160 = 160,
        [StringValue("PUNO")]
        prov161 = 161,
        [StringValue("AZANGARO")]
        prov162 = 162,
        [StringValue("CARABAYA")]
        prov163 = 163,
        [StringValue("CHUCUITO")]
        prov164 = 164,
        [StringValue("HUANCANE")]
        prov165 = 165,
        [StringValue("LAMPA")]
        prov166 = 166,
        [StringValue("MELGAR")]
        prov167 = 167,
        [StringValue("SANDIA")]
        prov168 = 168,
        [StringValue("SAN ROMAN")]
        prov169 = 169,
        [StringValue("YUNGUYO")]
        prov170 = 170,
        [StringValue("SAN ANTONIO DE PUTINA")]
        prov171 = 171,
        [StringValue("EL COLLAO")]
        prov172 = 172,
        [StringValue("MOHO")]
        prov173 = 173,
        [StringValue("MOYOBAMBA")]
        prov174 = 174,
        [StringValue("HUALLAGA")]
        prov175 = 175,
        [StringValue("LAMAS")]
        prov176 = 176,
        [StringValue("MARISCAL CACERES")]
        prov177 = 177,
        [StringValue("RIOJA")]
        prov178 = 178,
        [StringValue("SAN MARTIN")]
        prov179 = 179,
        [StringValue("BELLAVISTA")]
        prov180 = 180,
        [StringValue("TOCACHE")]
        prov181 = 181,
        [StringValue("PICOTA")]
        prov182 = 182,
        [StringValue("EL DORADO")]
        prov183 = 183,
        [StringValue("TACNA")]
        prov184 = 184,
        [StringValue("TARATA")]
        prov185 = 185,
        [StringValue("JORGE BASADRE")]
        prov186 = 186,
        [StringValue("CANDARAVE")]
        prov187 = 187,
        [StringValue("TUMBES")]
        prov188 = 188,
        [StringValue("CONTRALMIRANTE VILLAR")]
        prov189 = 189,
        [StringValue("ZARUMILLA")]
        prov190 = 190,
        [StringValue("CALLAO")]
        prov191 = 191,
        [StringValue("CORONEL PORTILLO")]
        prov192 = 192,
        [StringValue("PADRE ABAD")]
        prov193 = 193,
        [StringValue("ATALAYA")]
        prov194 = 194,
        [StringValue("PURUS")]
        prov195 = 195
    }

    public enum EnumTipoDocumento
    {
        [StringValue("---Seleccione Opción---")]
        TipoDoc0 = 0,
        [StringValue("DNI")]
        TipoDoc1 = 1,
        [StringValue("CARNÉ EXT.")]
        TipoDoc2 = 2,
        [StringValue("RUC")]
        TipoDoc3 = 3,
        [StringValue("PASAPORTE")]
        TipoDoc4 = 4,
        [StringValue("PART. NAC.")]
        TipoDoc5 = 5
    }

    public enum EnumCodTelef
    {
        [StringValue("---Seleccione Opción---")]
        cod0 = 0,
        [StringValue("1 / LIMA Y CALLAO")]
        cod1 = 1,
        [StringValue("41 / AMAZONAS")]
        cod2 = 2,
        [StringValue("42 / SAN MARTIN")]
        cod3 = 3,
        [StringValue("43 / ANCASH")]
        cod4 = 4,
        [StringValue("44 / LA LIBERTAD")]
        cod5 = 5,
        [StringValue("51 / PUNO")]
        cod6 = 6,
        [StringValue("52 / TACNA")]
        cod7 = 7,
        [StringValue("53 / MOQUEGUA")]
        cod8 = 8,
        [StringValue("54 / AREQUIPA")]
        cod9 = 9,
        [StringValue("56 / ICA")]
        cod10 = 10,
        [StringValue("61 / UCAYALI")]
        cod11 = 11,
        [StringValue("62 / HUANUCO")]
        cod12 = 12,
        [StringValue("63 / PASCO")]
        cod13 = 13,
        [StringValue("64/ JUNIN")]
        cod14 = 14,
        [StringValue("65 / LORETO")]
        cod15 = 15,
        [StringValue("66 / AYACUCHO")]
        cod16 = 16,
        [StringValue("67 / HUANCAVELICA")]
        cod17 = 17,
        [StringValue("72 / TUMBES")]
        cod18 = 18,
        [StringValue("73 / PIURA")]
        cod19 = 19,
        [StringValue("74 / LAMBAYEQUE")]
        cod20 = 20,
        [StringValue("76 / CAJAMARCA")]
        cod21 = 21,
        [StringValue("82 / MADRE DE DIOS")]
        cod22 = 22,
        [StringValue("83 / APURIMAC")]
        cod23 = 23,
        [StringValue("84 / CUSCO")]
        cod24 = 24
    }

    public enum EnumTipoZona
    {
        [StringValue("---Seleccione Opción---")]
        zona0 = 0,
        [StringValue("URB. URBANIZACIÓN")]
        zona1 = 1,
        [StringValue("P.J. PUEBLO JOVEN")]
        zona2 = 2,
        [StringValue("U.V. UNIDAD VECINAL")]
        zona3 = 3,
        [StringValue("C.H. CONJUNTO HABITACIONAL")]
        zona4 = 4,
        [StringValue("A.H. ASENTAMIENTO HUMANO")]
        zona5 = 5,
        [StringValue("COO. COOPERATIVA")]
        zona6 = 6,
        [StringValue("RES. RESIDENCIAL")]
        zona7 = 7,
        [StringValue("Z.I. ZONA INDUSTRIAL")]
        zona8 = 8,
        [StringValue("GRU. GRUPO")]
        zona9 = 9,
        [StringValue("CAS. CASERÍO")]
        zona10 = 10,
        [StringValue("FND. FUNDO")]
        zona11 = 11,
        [StringValue("OTROS")]
        zona12 = 12,
    }

    public enum EnumTipoVia
    {
        [StringValue("---Seleccione Opción---")]
        via0 = 0,
        [StringValue("AVENIDA")]
        via1 = 1,
        [StringValue("JIRÓN")]
        via2 = 2,
        [StringValue("CALLE")]
        via3 = 3,
        [StringValue("PASAJE")]
        via4 = 4,
        [StringValue("ALAMEDA")]
        via5 = 5,
        [StringValue("MALECÓN")]
        via6 = 6,
        [StringValue("OVALO")]
        via7 = 7,
        [StringValue("PARQUE")]
        via8 = 8,
        [StringValue("PLAZA")]
        via9 = 9,
        [StringValue("CARRETERA")]
        via10 = 10,
        [StringValue("TROCHA ")]
        via11 = 11,
        [StringValue("CAMINO RURAL")]
        via12 = 12,
        [StringValue("BAJADA")]
        via13 = 13,
        [StringValue("GALERIA ")]
        via14 = 14,
        [StringValue("PROLONGACIÓN ")]
        via15 = 15,
        [StringValue("PASEO")]
        via16 = 16,
        [StringValue("PLAZUELA")]
        via17 = 17,
        [StringValue("PORTAL")]
        via18 = 18,
        [StringValue("CAMINO AFIRMADO")]
        via19 = 19,
        [StringValue("TROCHA CARROZABLE")]
        via20 = 20,
        [StringValue("OTROS")]
        via21 = 21
    }

    public enum EnumTipoEmpleado
    {
        [StringValue("---Seleccione Opción---")]
        TipEmp0 = 0,
        [StringValue("EJECUTIVO")]
        TipEmp1 = 1,
        [StringValue("OBRERO")]
        TipEmp2 = 2,
        [StringValue("EMPLEADO")]
        TipEmp3 = 3,
        [StringValue("TRAB.PORTUARIO")]
        TipEmp4 = 4,
        [StringValue("PRACT. SENATI")]
        TipEmp5 = 5,
        [StringValue("PENSIONISTA O CESANTE")]
        TipEmp6 = 6,
        [StringValue("PENSIONISTA - LEY 28320")]
        TipEmp7 = 7,
        [StringValue("CONSTRUCCIÓN CIVIL")]
        TipEmp8 = 8,
        [StringValue("PILOTO Y COPIL DE AVIA. COM.")]
        TipEmp9 = 9,
        [StringValue("MARÍT, FLUVIAL O LACUSTRE")]
        TipEmp10 = 10,
        [StringValue("PERIODISTA ")]
        TipEmp11 = 11,
        [StringValue("TRAB. DE LA IND. DE CUERO")]
        TipEmp12 = 12,
        [StringValue("MINERO DE SOCAVÓN")]
        TipEmp13 = 13,
        [StringValue("PESCADOR - LEY 28320 ")]
        TipEmp14 = 14,
        [StringValue("MINERO DE TAJO ABIERTO ")]
        TipEmp15 = 15,
        [StringValue("MINERO IND. MINERA METAL. Y/O SIDERUR")]
        TipEmp16 = 16,
        [StringValue("ARTISTA -  LEY 28131")]
        TipEmp17 = 17,
        [StringValue("AGRARIO DEPEND.- LEY 27360")]
        TipEmp18 = 18,
        [StringValue("TRAB. ACTIV.ACUÍCOLA LEY 27460")]
        TipEmp19 = 19,
        [StringValue("PESC.Y PROC.ARTES.INDEP.")]
        TipEmp20 = 20,
        [StringValue("CONDUCT MICROEMP.REMYPE D.L.1086")]
        TipEmp21 = 21,
        [StringValue("CUARTA - QUINTA CATEGORÍA")]
        TipEmp22 = 22
    }

    public enum EnumMotivoBaja
    {
        [StringValue("---Seleccione Opción---")]
        MotBaja0 = 0,
        [StringValue("RENUNCIA")]
        MotBaja1 = 1,
        [StringValue("RENUNCIA CON INCENTIVOS")]
        MotBaja2 = 2,
        [StringValue("DESPIDO O DESTITUCIÓN")]
        MotBaja3 = 3,
        [StringValue("CESE COLECTIVO")]
        MotBaja4 = 4,
        [StringValue("JUBILACIÓN")]
        MotBaja5 = 5,
        [StringValue("INVALIDEZ ABSOLUTA PERMAN")]
        MotBaja6 = 6,
        [StringValue("TERMIN OBRA/SERV, CUMPLIM CONDIC RESOL. O VENC PLAZO")]
        MotBaja7 = 7,
        [StringValue("MUTUO DISENSO")]
        MotBaja8 = 8,
        [StringValue("FALLECIMIENTO")]
        MotBaja9 = 9,
        [StringValue("SUSPENSIÓN DE LA PENSIÓN")]
        MotBaja10 = 10,
        [StringValue("REASIGNACIÓN")]
        MotBaja11 = 11,
        [StringValue("PERMUTA")]
        MotBaja12 = 12,
        [StringValue("TRANSFERENCIA")]
        MotBaja13 = 13,
        [StringValue("BAJA POR SUC. EN POSIC DEL EMPLEADOR")]
        MotBaja14 = 14,
        [StringValue("EXTINCIÓN O LIQUID. DEL EMPLEADOR")]
        MotBaja15 = 15,
        [StringValue("OTR MOTIV CADUC PENSIÓN ")]
        MotBaja16 = 16,
        [StringValue("NO SE INICIÓ LA REL.  LABORAL O PREST. DE SERVICIOS")]
        MotBaja17 = 17
    }

    public enum EnumRegLaboral
    {
        [StringValue("---Seleccione Opción---")]
        RegLab0 = 0,
        [StringValue("D LEG N.° 728")]
        RegLab1 = 1,
        [StringValue("MICROEMPRESA")]
        RegLab2 = 2,
        [StringValue("PEQUEÑA EMPRESA")]
        RegLab3 = 3,
        [StringValue("AGRARIO")]
        RegLab4 = 4,
        [StringValue("EXPORTACION NO TRADICIONAL")]
        RegLab5 = 5,
        [StringValue("MINEROS")]
        RegLab6 = 6,
        [StringValue("CONSTRUCCION CIVIL")]
        RegLab7 = 7,
        [StringValue("OTROS")]
        RegLab8 = 8
    }

    public enum EnumRegSalud
    {
        [StringValue("---Seleccione Opción---")]
        RegSalud0 = 0,
        [StringValue("ESSALUD REGULAR (Exclusivamente)")]
        RegSalud1 = 1,
        [StringValue("ESSALUD REGULAR Y EPS/SERV. PROPIOS")]
        RegSalud2 = 2,
        [StringValue("ESSALUD TRABAJADORES PESQUEROS")]
        RegSalud3 = 3,
        [StringValue("ESSALUD TRABAJADORES PESQUEROS Y EPS(SERV.PROPIOS)")]
        RegSalud4 = 4,
        [StringValue("ESSALUD AGRARIO/ACUÍCOLA")]
        RegSalud5 = 5,
        [StringValue("ESSALUD PENSIONISTAS")]
        RegSalud6 = 6,
        [StringValue("SANIDAD DE FFAA Y POLICIALES")]
        RegSalud7 = 7,
        [StringValue("SIS – MICROEMPRESA")]
        RegSalud8 = 8
    }

    public enum EnumRegPension
    {
        [StringValue("---Seleccione Opción---")]
        RegPen0 = 0,
        [StringValue("DL 19990 - SIST NAC DE PENS - ONP")]
        RegPen1 = 1,
        [StringValue("CBSSP")]
        RegPen2 = 2,
        [StringValue("OTROS REGIMENES PENSIONARIOS")]
        RegPen3 = 3,
        [StringValue("SPP INTEGRA")]
        RegPen4 = 4,
        [StringValue("SPP HORIZONTE")]
        RegPen5 = 5,
        [StringValue("SPP PROFUTURO")]
        RegPen6 = 6,
        [StringValue("SPP PRIMA")]
        RegPen7 = 7,
        [StringValue("SIN REGIMEN PENSIONARIO")]
        RegPen8 = 8
    }

    public enum EnumCatOcupacional
    {
        [StringValue("---Seleccione Opción---")]
        RegPen0 = 0,
        [StringValue("EJECUTIVO")]
        Plan_3Month = 1,
        [StringValue("OBRERO")]
        Plan_6Month = 2,
        [StringValue("EMPLEADO")]
        Plan_12Month = 3
    }

    public enum EnumNivelEducativo
    {
        [StringValue("---Seleccione Opción---")]
        Educa0 = 0,
        [StringValue("SIN EDUCACIÓN FORMAL")]
        Educa1 = 1,
        [StringValue("ESPECIAL INCOMPLETA")]
        Educa2 = 2,
        [StringValue("ESPECIAL COMPLETA")]
        Educa3 = 3,
        [StringValue("PRIMARIA INCOMPLETA")]
        Educa4 = 4,
        [StringValue("PRIMARIA COMPLETA")]
        Educa5 = 5,
        [StringValue("SECUNDARIA INCOMPLETA")]
        Educa6 = 6,
        [StringValue("SECUNDARIA COMPLETA")]
        Educa7 = 7,
        [StringValue("TÉCNICA INCOMPLETA")]
        Educa8 = 8,
        [StringValue("TÉCNICA COMPLETA")]
        Educa9 = 9,
        [StringValue("SUPERIOR INCOMPLETA (INSTIT. SUPER)")]
        Educa10 = 10,
        [StringValue("SUPERIOR COMPLETA  (INSTIT SUPER) ")]
        Educa11 = 11,
        [StringValue("UNIVERSITARIA INCOMPLETA")]
        Educa12 = 12,
        [StringValue("UNIVERSITARIA COMPLETA")]
        Educa13 = 13,
        [StringValue("GRADO DE BACHILLER")]
        Educa14 = 14,
        [StringValue("TITULADO")]
        Educa15 = 15,
        [StringValue("ESTUD. MAESTRÍA INCOMPLETA")]
        Educa16 = 16,
        [StringValue("ESTUD. MAESTRÍA COMPLETA")]
        Educa17 = 17,
        [StringValue("GRADO DE MAESTRÍA")]
        Educa18 = 18,
        [StringValue("ESTUD. DOCTORADO INCOMPLETO")]
        Educa19 = 19,
        [StringValue("ESTUD. DOCTORADO COMPLETO")]
        Educa20 = 20,
        [StringValue("GRADO DE DOCTOR")]
        Educa21 = 21
    }

    public enum EnumTipoPago
    {
        [StringValue("---Seleccione Opción---")]
        TipPago0 = 0,
        [StringValue("EFECTIVO")]
        TipPago1 = 1,
        [StringValue("DEPÓSITO EN CUENTA")]
        TipPago2 = 2,
        [StringValue("OTROS")]
        TipPago3 = 3
    }

    public enum EnumPeriodicidad
    {
        [StringValue("---Seleccione Opción---")]
        Periodo0 = 0,
        [StringValue("MENSUAL")]
        Periodo1 = 1,
        [StringValue("QUINCENAL")]
        Periodo2 = 2,
        [StringValue("SEMANAL")]
        Periodo3 = 3,
        [StringValue("DIARIA")]
        Periodo4 = 4,
        [StringValue("OTROS")]
        Periodo5 = 5
    }

    //public enum EnumTipoContrato
    //{
    //    [StringValue("---Seleccione Opción---")]
    //    TipContr0 = 0,
    //    [StringValue("A PLAZO INDET - D.LEG. 728")]
    //    TipContr1 = 1,
    //    [StringValue("A TIEMPO PARCIAL")]
    //    TipContr2 = 2,
    //    [StringValue("POR INICIO O INCREM DE ACTIV")]
    //    TipContr3 = 3,
    //    [StringValue("POR NECESIDADES DEL MERCADO")]
    //    TipContr4 = 4,
    //    [StringValue("POR RECONVERSIÓN EMPRESARIAL")]
    //    TipContr5 = 5,
    //    [StringValue("OCASIONAL")]
    //    TipContr6 = 6,
    //    [StringValue("DE SUPLENCIA")]
    //    TipContr7 = 7,
    //    [StringValue("DE EMERGENCIA")]
    //    TipContr8 = 8,
    //    [StringValue("PARA OBRA DETERMINADA O SERVICIO ESPECÍFICO")]
    //    TipContr9 = 9,
    //    [StringValue("INTERMITENTE")]
    //    TipContr10 = 10,
    //    [StringValue("DE TEMPORADA ")]
    //    TipContr11 = 11,
    //    [StringValue("DE EXPORTACIÓN NO TRADICIONAL D.LEY 22342")]
    //    TipContr12 = 12,
    //    [StringValue("DE EXTRANJERO - D.LEG.689")]
    //    TipContr13 = 13,
    //    [StringValue("ADMINISTRATIVO DE SERVICIOS - D.LEG 1057")]
    //    TipContr14 = 14,
    //    [StringValue("NOMBRADO - D.LEG. N.° 276")]
    //    TipContr15 = 15,
    //    [StringValue("SERVICIOS PERSONALES  - APLICABLES A LOS REGÍM. DE CARRERA")]
    //    TipContr16 = 16,
    //    [StringValue("GERENTE PÚBLICO - D.LEG. 1024")]
    //    TipContr17 = 17,
    //    [StringValue("A DOMICILIO")]
    //    TipContr18 = 18,
    //    [StringValue("FUTBOLISTAS PROFESIONALES")]
    //    TipContr19 = 19,
    //    [StringValue("AGRARIO - LEY 27360")]
    //    TipContr20 = 20,
    //    [StringValue("MIGRANTE ANDINO DECISIÓN 545")]
    //    TipContr21 = 21,
    //    [StringValue("OTROS NO PREVISTOS")]
    //    TipContr22 = 22
    //}

    public enum EnumSituacionTrab
    {
        [StringValue("---Seleccione Opción---")]
        SitTrabajador0 = 0,
        [StringValue("BAJA")]
        SitTrabajador1 = 1,
        [StringValue("ACTIVO O SUBSIDIADO")]
        SitTrabajador2 = 2,
        [StringValue("SIN VÍNCULO LABORAL CON CONCEPTOS PENDIENTE DE LIQUIDAR")]
        SitTrabajador3 = 3,
        [StringValue("SUSPENSIÓN PERFECTA DE LABORES")]
        SitTrabajador4 = 4
    }
    public enum EnumComision
    {
        [StringValue("---Seleccione Opción---")]
        SitTrabajador0 = 0,
        [StringValue("COMISION MIXTA")]
        SitTrabajador1 = 1,
        [StringValue("COMISION FLUJO")]
        SitTrabajador2 = 2,
    }

    public enum EnumEstadoTrab
    {
        [StringValue("PENDIENTE")]
        SitTrabajador0 = 0,
        [StringValue("BAJA")]
        SitTrabajador1 = 1,
        [StringValue("ACTIVO")]
        SitTrabajador2 = 2
    }

    public enum EnumConvenios
    {
        [StringValue("NINGUNO")]
        Convenio1 = 1,
        [StringValue("CANADA")]
        Convenio2 = 2,
        [StringValue("CHILE")]
        Convenio3 = 3,
        [StringValue("CAN ")]
        Convenio4 = 4,
        [StringValue("BRASIL")]
        Convenio5 = 5
    }

    public enum EnumModalidadFormat
    {
        [StringValue("---Seleccione Opción---")]
        ModalFor0 = 0,
        [StringValue("APRENDIZAJE CON PREDOMINIO EN LA EMPRESA")]
        ModalFor1 = 1,
        [StringValue("APRENDIZAJE CON PREDOMINIO EN EL CFP - PRÁCTICAS P")]
        ModalFor2 = 2,
        [StringValue("PRÁCTICAS PROFESIONALES")]
        ModalFor3 = 3,
        [StringValue("CAPACITACIÓN LABORAL JUVENIL")]
        ModalFor4 = 4,
        [StringValue("PASANTÍA EN LA EMPRESA")]
        ModalFor5 = 5,
        [StringValue("PASANTÍA DE DOCENTES Y CATEDRÁTICOS")]
        ModalFor6 = 6,
        [StringValue("ACTUALIZACIÓN PARA LA REINSERCIÓN LABORAL")]
        ModalFor7 = 7,
        [StringValue("SECIGRISTA")]
        ModalFor8 = 8
    }

    public enum EnumNacionalidad
    {
        [StringValue("---Seleccione Opción---")]
        Nacion0 = 0,
        [StringValue("BOUVET ISLAND")]
        Nacion1 = 1,
        [StringValue("COTE D IVOIRE")]
        Nacion2 = 2,
        [StringValue("FALKLAND ISLANDS (MALVINAS)")]
        Nacion3 = 3,
        [StringValue("FRANCE, METROPOLITAN")]
        Nacion4 = 4,
        [StringValue("FRENCH SOUTHERN TERRITORIES")]
        Nacion5 = 5,
        [StringValue("HEARD AND MC DONALD ISLANDS")]
        Nacion6 = 6,
        [StringValue("MAYOTTE")]
        Nacion7 = 7,
        [StringValue("SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS")]
        Nacion8 = 8,
        [StringValue("SVALBARD AND JAN MAYEN ISLANDS")]
        Nacion9 = 9,
        [StringValue("UNITED STATES MINOR OUTLYING ISLANDS")]
        Nacion10 = 10,
        [StringValue("OTROS PAISES O LUGARES")]
        Nacion11 = 11,
        [StringValue("AFGANISTAN")]
        Nacion12 = 12,
        [StringValue("ALBANIA")]
        Nacion13 = 13,
        [StringValue("ALDERNEY")]
        Nacion14 = 14,
        [StringValue("ALEMANIA")]
        Nacion15 = 15,
        [StringValue("ARMENIA")]
        Nacion16 = 16,
        [StringValue("ARUBA")]
        Nacion17 = 17,
        [StringValue("ASCENCION")]
        Nacion18 = 18,
        [StringValue("BOSNIA-HERZEGOVINA")]
        Nacion19 = 19,
        [StringValue("BURKINA FASO")]
        Nacion20 = 20,
        [StringValue("ANDORRA")]
        Nacion21 = 21,
        [StringValue("ANGOLA")]
        Nacion22 = 22,
        [StringValue("ANGUILLA")]
        Nacion23 = 23,
        [StringValue("ANTIGUA Y BARBUDA")]
        Nacion24 = 24,
        [StringValue("ANTILLAS HOLANDESAS")]
        Nacion25 = 25,
        [StringValue("ARABIA SAUDITA")]
        Nacion26 = 26,
        [StringValue("ARGELIA")]
        Nacion27 = 27,
        [StringValue("ARGENTINA")]
        Nacion28 = 28,
        [StringValue("AUSTRALIA")]
        Nacion29 = 29,
        [StringValue("AUSTRIA")]
        Nacion30 = 30,
        [StringValue("AZERBAIJÁN")]
        Nacion31 = 31,
        [StringValue("BAHAMAS")]
        Nacion32 = 32,
        [StringValue("BAHREIN")]
        Nacion33 = 33,
        [StringValue("BANGLA DESH")]
        Nacion34 = 34,
        [StringValue("BARBADOS")]
        Nacion35 = 35,
        [StringValue("BÉLGICA")]
        Nacion36 = 36,
        [StringValue("BELICE")]
        Nacion37 = 37,
        [StringValue("BERMUDAS")]
        Nacion38 = 38,
        [StringValue("BELARUS")]
        Nacion39 = 39,
        [StringValue("MYANMAR")]
        Nacion40 = 40,
        [StringValue("BOLIVIA")]
        Nacion41 = 41,
        [StringValue("BOTSWANA")]
        Nacion42 = 42,
        [StringValue("BRASIL")]
        Nacion43 = 43,
        [StringValue("BRUNEI DARUSSALAM")]
        Nacion44 = 44,
        [StringValue("BULGARIA")]
        Nacion45 = 45,
        [StringValue("BURUNDI")]
        Nacion46 = 46,
        [StringValue("BUTÁN")]
        Nacion47 = 47,
        [StringValue("CABO VERDE")]
        Nacion48 = 48,
        [StringValue("CAIMÁN, ISLAS")]
        Nacion49 = 49,
        [StringValue("CAMBOYA")]
        Nacion50 = 50,
        [StringValue("CAMERÚN, REPUBLICA UNIDA DEL")]
        Nacion51 = 51,
        [StringValue("CAMPIONE D TALIA")]
        Nacion52 = 52,
        [StringValue("CANADÁ")]
        Nacion53 = 53,
        [StringValue("CANAL (NORMANDAS), ISLAS")]
        Nacion54 = 54,
        [StringValue("CANTÓN Y ENDERBURRY")]
        Nacion55 = 55,
        [StringValue("SANTA SEDE")]
        Nacion56 = 56,
        [StringValue("COCOS (KEELING),ISLAS")]
        Nacion57 = 57,
        [StringValue("COLOMBIA")]
        Nacion58 = 58,
        [StringValue("COMORAS")]
        Nacion59 = 59,
        [StringValue("CONGO")]
        Nacion60 = 60,
        [StringValue("COOK, ISLAS")]
        Nacion61 = 61,
        [StringValue("COREA (NORTE), REPUBLICA POPULAR DEMOCRATICA DE")]
        Nacion62 = 62,
        [StringValue("COREA (SUR), REPUBLICA DE")]
        Nacion63 = 63,
        [StringValue("COSTA DE MARFIL")]
        Nacion64 = 64,
        [StringValue("COSTA RICA")]
        Nacion65 = 65,
        [StringValue("CROACIA")]
        Nacion66 = 66,
        [StringValue("CUBA")]
        Nacion67 = 67,
        [StringValue("CHAD")]
        Nacion68 = 68,
        [StringValue("CHECOSLOVAQUIA")]
        Nacion69 = 69,
        [StringValue("CHILE")]
        Nacion70 = 70,
        [StringValue("CHINA")]
        Nacion71 = 71,
        [StringValue("TAIWAN (FORMOSA)")]
        Nacion72 = 72,
        [StringValue("CHIPRE")]
        Nacion73 = 73,
        [StringValue("BENIN")]
        Nacion74 = 74,
        [StringValue("DINAMARCA")]
        Nacion75 = 75,
        [StringValue("DOMINICA")]
        Nacion76 = 76,
        [StringValue("ECUADOR")]
        Nacion77 = 77,
        [StringValue("EGIPTO")]
        Nacion78 = 78,
        [StringValue("EL SALVADOR")]
        Nacion79 = 79,
        [StringValue("ERITREA")]
        Nacion80 = 80,
        [StringValue("EMIRATOS ARABES UNIDOS")]
        Nacion81 = 81,
        [StringValue("ESPAÑA")]
        Nacion82 = 82,
        [StringValue("ESLOVAQUIA")]
        Nacion83 = 83,
        [StringValue("ESLOVENIA")]
        Nacion84 = 84,
        [StringValue("ESTADOS UNIDOS")]
        Nacion85 = 85,
        [StringValue("ESTONIA")]
        Nacion86 = 86,
        [StringValue("ETIOPIA")]
        Nacion87 = 87,
        [StringValue("FEROE, ISLAS")]
        Nacion88 = 88,
        [StringValue("FILIPINAS")]
        Nacion89 = 89,
        [StringValue("FINLANDIA")]
        Nacion90 = 90,
        [StringValue("FRANCIA")]
        Nacion91 = 91,
        [StringValue("GABON")]
        Nacion92 = 92,
        [StringValue("GAMBIA")]
        Nacion93 = 93,
        [StringValue("GAZA Y JERICO")]
        Nacion94 = 94,
        [StringValue("GEORGIA")]
        Nacion95 = 95,
        [StringValue("GHANA")]
        Nacion96 = 96,
        [StringValue("GIBRALTAR")]
        Nacion97 = 97,
        [StringValue("GRANADA")]
        Nacion98 = 98,
        [StringValue("GRECIA")]
        Nacion99 = 99,
        [StringValue("GROENLANDIA")]
        Nacion100 = 100,
        [StringValue("GUADALUPE")]
        Nacion101 = 101,
        [StringValue("GUAM")]
        Nacion102 = 102,
        [StringValue("GUATEMALA")]
        Nacion103 = 103,
        [StringValue("GUAYANA FRANCESA")]
        Nacion104 = 104,
        [StringValue("GUERNSEY")]
        Nacion105 = 105,
        [StringValue("GUINEA")]
        Nacion106 = 106,
        [StringValue("GUINEA ECUATORIAL")]
        Nacion107 = 107,
        [StringValue("GUINEA-BISSAU")]
        Nacion108 = 108,
        [StringValue("GUYANA")]
        Nacion109 = 109,
        [StringValue("HAITI")]
        Nacion110 = 110,
        [StringValue("HONDURAS")]
        Nacion111 = 111,
        [StringValue("HONDURAS BRITANICAS")]
        Nacion112 = 112,
        [StringValue("HONG KONG")]
        Nacion113 = 113,
        [StringValue("HUNGRIA")]
        Nacion114 = 114,
        [StringValue("INDIA")]
        Nacion115 = 115,
        [StringValue("INDONESIA")]
        Nacion116 = 116,
        [StringValue("IRAK")]
        Nacion117 = 117,
        [StringValue("IRAN, REPUBLICA ISLAMICA DEL")]
        Nacion118 = 118,
        [StringValue("IRLANDA (EIRE)")]
        Nacion119 = 119,
        [StringValue("ISLA AZORES")]
        Nacion120 = 120,
        [StringValue("ISLA DEL MAN")]
        Nacion121 = 121,
        [StringValue("ISLANDIA")]
        Nacion122 = 122,
        [StringValue("ISLAS CANARIAS")]
        Nacion123 = 123,
        [StringValue("ISLAS DE CHRISTMAS")]
        Nacion124 = 124,
        [StringValue("ISLAS QESHM")]
        Nacion125 = 125,
        [StringValue("ISRAEL")]
        Nacion126 = 126,
        [StringValue("ITALIA")]
        Nacion127 = 127,
        [StringValue("JAMAICA")]
        Nacion128 = 128,
        [StringValue("JONSTON, ISLAS")]
        Nacion129 = 129,
        [StringValue("JAPON")]
        Nacion130 = 130,
        [StringValue("JERSEY")]
        Nacion131 = 131,
        [StringValue("JORDANIA")]
        Nacion132 = 132,
        [StringValue("KAZAJSTAN")]
        Nacion133 = 133,
        [StringValue("KENIA")]
        Nacion134 = 134,
        [StringValue("KIRIBATI")]
        Nacion135 = 135,
        [StringValue("KIRGUIZISTAN")]
        Nacion136 = 136,
        [StringValue("KUWAIT")]
        Nacion137 = 137,
        [StringValue("LABUN")]
        Nacion138 = 138,
        [StringValue("LAOS, REPUBLICA POPULAR DEMOCRATICA DE")]
        Nacion139 = 139,
        [StringValue("LESOTHO")]
        Nacion140 = 140,
        [StringValue("LETONIA")]
        Nacion141 = 141,
        [StringValue("LIBANO")]
        Nacion142 = 142,
        [StringValue("LIBERIA")]
        Nacion143 = 143,
        [StringValue("LIBIA")]
        Nacion144 = 144,
        [StringValue("LIECHTENSTEIN")]
        Nacion145 = 145,
        [StringValue("LITUANIA")]
        Nacion146 = 146,
        [StringValue("LUXEMBURGO")]
        Nacion147 = 147,
        [StringValue("MACAO")]
        Nacion148 = 148,
        [StringValue("MACEDONIA")]
        Nacion149 = 149,
        [StringValue("MADAGASCAR")]
        Nacion150 = 150,
        [StringValue("MADEIRA")]
        Nacion151 = 151,
        [StringValue("MALAYSIA")]
        Nacion152 = 152,
        [StringValue("MALAWI")]
        Nacion153 = 153,
        [StringValue("MALDIVAS")]
        Nacion154 = 154,
        [StringValue("MALI")]
        Nacion155 = 155,
        [StringValue("MALTA")]
        Nacion156 = 156,
        [StringValue("MARIANAS DEL NORTE, ISLAS")]
        Nacion157 = 157,
        [StringValue("MARSHALL, ISLAS")]
        Nacion158 = 158,
        [StringValue("MARRUECOS")]
        Nacion159 = 159,
        [StringValue("MARTINICA")]
        Nacion160 = 160,
        [StringValue("MAURICIO")]
        Nacion161 = 161,
        [StringValue("MAURITANIA")]
        Nacion162 = 162,
        [StringValue("MEXICO")]
        Nacion163 = 163,
        [StringValue("MICRONESIA, ESTADOS FEDERADOS DE")]
        Nacion164 = 164,
        [StringValue("MIDWAY ISLAS")]
        Nacion165 = 165,
        [StringValue("MOLDAVIA")]
        Nacion166 = 166,
        [StringValue("MONGOLIA")]
        Nacion167 = 167,
        [StringValue("MONACO")]
        Nacion168 = 168,
        [StringValue("MONTSERRAT, ISLA")]
        Nacion169 = 169,
        [StringValue("MOZAMBIQUE")]
        Nacion170 = 170,
        [StringValue("NAMIBIA")]
        Nacion171 = 171,
        [StringValue("NAURU")]
        Nacion172 = 172,
        [StringValue("NAVIDAD (CHRISTMAS), ISLA")]
        Nacion173 = 173,
        [StringValue("NEPAL")]
        Nacion174 = 174,
        [StringValue("NICARAGUA")]
        Nacion175 = 175,
        [StringValue("NIGER")]
        Nacion176 = 176,
        [StringValue("NIGERIA")]
        Nacion177 = 177,
        [StringValue("NIUE, ISLA")]
        Nacion178 = 178,
        [StringValue("NORFOLK, ISLA")]
        Nacion179 = 179,
        [StringValue("NORUEGA")]
        Nacion180 = 180,
        [StringValue("NUEVA CALEDONIA")]
        Nacion181 = 181,
        [StringValue("PAPUASIA NUEVA GUINEA")]
        Nacion182 = 182,
        [StringValue("NUEVA ZELANDA")]
        Nacion183 = 183,
        [StringValue("VANUATU")]
        Nacion184 = 184,
        [StringValue("OMAN")]
        Nacion185 = 185,
        [StringValue("PACIFICO, ISLAS DEL")]
        Nacion186 = 186,
        [StringValue("PAISES BAJOS")]
        Nacion187 = 187,
        [StringValue("PAKISTAN")]
        Nacion188 = 188,
        [StringValue("PALAU, ISLAS")]
        Nacion189 = 189,
        [StringValue("TERRITORIO AUTONOMO DE PALESTINA.")]
        Nacion190 = 190,
        [StringValue("PANAMA")]
        Nacion191 = 191,
        [StringValue("PARAGUAY")]
        Nacion192 = 192,
        [StringValue("PERÚ")]
        Nacion193 = 193,
        [StringValue("PITCAIRN, ISLA")]
        Nacion194 = 194,
        [StringValue("POLINESIA FRANCESA")]
        Nacion195 = 195,
        [StringValue("POLONIA")]
        Nacion196 = 196,
        [StringValue("PORTUGAL")]
        Nacion197 = 197,
        [StringValue("PUERTO RICO")]
        Nacion198 = 198,
        [StringValue("QATAR")]
        Nacion199 = 199,
        [StringValue("REINO UNIDO")]
        Nacion200 = 200,
        [StringValue("ESCOCIA")]
        Nacion201 = 201,
        [StringValue("REPUBLICA ARABE UNIDA")]
        Nacion202 = 202,
        [StringValue("REPUBLICA CENTROAFRICANA")]
        Nacion203 = 203,
        [StringValue("REPUBLICA CHECA")]
        Nacion204 = 204,
        [StringValue("REPUBLICA DE SWAZILANDIA")]
        Nacion205 = 205,
        [StringValue("REPUBLICA DE TUNEZ")]
        Nacion206 = 206,
        [StringValue("REPUBLICA DOMINICANA")]
        Nacion207 = 207,
        [StringValue("REUNION")]
        Nacion208 = 208,
        [StringValue("ZIMBABWE")]
        Nacion209 = 209,
        [StringValue("RUMANIA")]
        Nacion210 = 210,
        [StringValue("RUANDA")]
        Nacion211 = 211,
        [StringValue("RUSIA")]
        Nacion212 = 212,
        [StringValue("SALOMON, ISLAS")]
        Nacion213 = 213,
        [StringValue("SAHARA OCCIDENTAL")]
        Nacion214 = 214,
        [StringValue("SAMOA OCCIDENTAL")]
        Nacion215 = 215,
        [StringValue("SAMOA NORTEAMERICANA")]
        Nacion216 = 216,
        [StringValue("SAN CRISTOBAL Y NIEVES")]
        Nacion217 = 217,
        [StringValue("SAN MARINO")]
        Nacion218 = 218,
        [StringValue("SAN PEDRO Y MIQUELON")]
        Nacion219 = 219,
        [StringValue("SAN VICENTE Y LAS GRANADINAS")]
        Nacion220 = 220,
        [StringValue("SANTA ELENA")]
        Nacion221 = 221,
        [StringValue("SANTA LUCIA")]
        Nacion222 = 222,
        [StringValue("SANTO TOME Y PRINCIPE")]
        Nacion223 = 223,
        [StringValue("SENEGAL")]
        Nacion224 = 224,
        [StringValue("SEYCHELLES")]
        Nacion225 = 225,
        [StringValue("SIERRA LEONA")]
        Nacion226 = 226,
        [StringValue("SINGAPUR")]
        Nacion227 = 227,
        [StringValue("SIRIA, REPUBLICA ARABE DE")]
        Nacion228 = 228,
        [StringValue("SOMALIA")]
        Nacion229 = 229,
        [StringValue("SRI LANKA")]
        Nacion230 = 230,
        [StringValue("SUDAFRICA, REPUBLICA DE")]
        Nacion231 = 231,
        [StringValue("SUDAN")]
        Nacion232 = 232,
        [StringValue("SUECIA")]
        Nacion233 = 233,
        [StringValue("SUIZA")]
        Nacion234 = 234,
        [StringValue("SURINAM")]
        Nacion235 = 235,
        [StringValue("SAWSILANDIA")]
        Nacion236 = 236,
        [StringValue("TADJIKISTAN")]
        Nacion237 = 237,
        [StringValue("TAILANDIA")]
        Nacion238 = 238,
        [StringValue("TANZANIA, REPUBLICA UNIDA DE")]
        Nacion239 = 239,
        [StringValue("DJIBOUTI")]
        Nacion240 = 240,
        [StringValue("TERRITORIO ANTARTICO BRITANICO")]
        Nacion241 = 241,
        [StringValue("TERRITORIO BRITANICO DEL OCEANO INDICO")]
        Nacion242 = 242,
        [StringValue("TIMOR DEL ESTE")]
        Nacion243= 243,
        [StringValue("TOGO")]
        Nacion244 = 244,
        [StringValue("TOKELAU")]
        Nacion245 = 245,
        [StringValue("TONGA")]
        Nacion246 = 246,
        [StringValue("TRINIDAD Y TOBAGO")]
        Nacion247 = 247,
        [StringValue("TRISTAN DA CUNHA")]
        Nacion248 = 248,
        [StringValue("TUNICIA")]
        Nacion249 = 249,
        [StringValue("TURCAS Y CAICOS, ISLAS")]
        Nacion250 = 250,
        [StringValue("TURKMENISTAN")]
        Nacion251 = 251,
        [StringValue("TURQUIA")]
        Nacion252 = 252,
        [StringValue("TUVALU")]
        Nacion253 = 253,
        [StringValue("UCRANIA")]
        Nacion254 = 254,
        [StringValue("UGANDA")]
        Nacion255 = 255,
        [StringValue("URSS")]
        Nacion256 = 256,
        [StringValue("URUGUAY")]
        Nacion257 = 257,
        [StringValue("UZBEKISTAN")]
        Nacion258 = 258,
        [StringValue("VENEZUELA")]
        Nacion259 = 259,
        [StringValue("VIET NAM")]
        Nacion260 = 260,
        [StringValue("VIETNAM (DEL NORTE)")]
        Nacion261 = 261,
        [StringValue("VIRGENES, ISLAS (BRITANICAS)")]
        Nacion262 = 262,
        [StringValue("VIRGENES, ISLAS (NORTEAMERICANAS)")]
        Nacion263 = 263,
        [StringValue("FIJI")]
        Nacion264 = 264,
        [StringValue("WAKE, ISLA")]
        Nacion265 = 265,
        [StringValue("WALLIS Y FORTUNA, ISLAS")]
        Nacion266 = 266,
        [StringValue("YEMEN")]
        Nacion267 = 267,
        [StringValue("YUGOSLAVIA")]
        Nacion268 = 268,
        [StringValue("ZAIRE")]
        Nacion269 = 269,
        [StringValue("ZAMBIA")]
        Nacion270 = 270,
        [StringValue("ZONA DEL CANAL DE PANAMA")]
        Nacion271 = 271,
        [StringValue("ZONA LIBRE OSTRAVA")]
        Nacion272 = 272,
        [StringValue("ZONA NEUTRAL (PALESTINA)")]
        Nacion273 = 273
    }


    public enum EnumPlanPayment
    {
        [StringValue("Plan_1Month")]
        Plan_1Month = 10,
        [StringValue("Plan_3Month")]
        Plan_3Month = 20,
        [StringValue("Plan_6Month")]
        Plan_6Month = 30,
        [StringValue("Plan_12Month")]
        Plan_12Month = 12
    }

   
    public enum EnumControlType
    {
        [StringValue("input")]
        Input = 1,
        [StringValue("textarea")]
        InputLarge = 2,
        [StringValue("ddl")]
        DropDownList = 3,
        [StringValue("url")]
        Url = 4,
        [StringValue("fileupload")]
        FileUpload = 5
    }

    public enum EnumEmailTemplates
    {

        Reminder = 2,
        ApproveYes = 7,
        ApproveNo = 8,
        Invited = 1050,
        welcome = 2057,
        Register = 2058,
        NewParty = 2059,
        Thank = 2062,
        Missed = 2063

    }


    public enum EnumEmailAccess
    {
        [StringValue("mastermail@tru-friends.com|k+aRJKKGcsOsxh3P5u3gcQ==|Tru-Friends")]
        Mastermail = 1,
        [StringValue("billing@tru-friends.com|LAI7MxUXVD+KVMNFqFAAnCpWoe6JqALA4hWKI/0KoWI=|Tru-Friends Billing")]
        Billing = 2
    }

    public enum EnumEmailStatus
    {
        Success = 1,
        Error = 2,
        Cancelled = 3
    }

    public enum EnumFormType
    {
        EntryForm = 1,
        JudgeForm = 2,
        TemplateForm = 3
    }

    public enum EnumTypeEmailTemplates
    {
        PartyPlan = 1,
        CMS = 2,
        Notifications = 3,
        Responders = 4
    }



    public enum EnumMimeType
    {
        [StringValue("Document")]
        Document = 1,
        [StringValue("Image")]
        Image = 2,
        [StringValue("Video")]
        Video = 3,
        [StringValue("Audio")]
        Audio = 4,
        [StringValue("Presentation")]
        Presentation = 5
    }

    public enum EnumAccessType
    {
        [StringValue("Standard")]
        Standard = 1,
        [StringValue("Admin")]
        Admin = 2
    }

    public enum EnumLoginAccessType
    {
        [StringValue("All")]
        All = -1,
        [StringValue("Admin")]
        Admin = 10,
        [StringValue("Distributor")]
        Distributor = 20
    }

    public enum EnumTypeResource
    {
        [StringValue("Story")]
        Story = 0,
        [StringValue("Photo")]
        Photo = 1
    }
    public enum EnumSettingAccess
    {
        [StringValue("Roles")]
        Roles = 0,
        [StringValue("Pages")]
        Pages = 1,
        [StringValue("Report")]
        Report = 2,
    }

    public enum EnumAccessQuery
    {
        [StringValue("SuperADmin")]
        Admin = -1
    }
    public enum EnumLibrary
    {
        [StringValue("Video")]
        Video = 1,
        [StringValue("Brochure")]
        Brochure = 2,
        [StringValue("Document")]
        Document = 3,
        [StringValue("FAQ")]
        FAQ = 4
    }
    public enum EnumExportFileExtension
    {
        [StringValue("csv")]
        csv = 1,
        [StringValue("xls")]
        xls = 2,
        [StringValue("txt")]
        txt = 3        
    }
    public enum EnumExportDelimiter
    {
        [StringValue(",(comma)")]
        comma = 1,
        [StringValue("|(pipe)")]
        pipe = 2,
        [StringValue(":(colon)")]
        colon = 3,
        [StringValue("TAB")]
        TAB = 4
    }
    public enum EnumExportEncapsulation
    {
        [StringValue("'(single quote)")]
        singlequote = 1,
        [StringValue("*(double quote)")]
        doublequote = 2,
        [StringValue("None")]
        none = 3
    }
    public enum EnumExportType
    {
        [StringValue("Flat File")]
        Flat = 0,
        [StringValue("3PL Company A")]
        CompanyA = 1,
        [StringValue("3PL Company B")]
        CompanyB = 2

    }
    public enum EnumLibrarySectionFAQ
    {
        [StringValue("Library")]
        Library = 0,
        [StringValue("ASEA")]
        ASEA = 1,
        [StringValue("RENU28")]
        RENU28 = 2,
        
    }
    public enum EnumMessageError
    {
        [StringValue("Not authorized.")]
        Notauthorized = 1,
        [StringValue("Error in sending data")]
        Errorsendingdata = 2
    }
    public enum EnumFolderSettings
    {
        [StringValue("export\\")]
        FolderExport = 1,
        [StringValue("resources\\")]
        FolderResources = 2,
        [StringValue("Profile_User\\")]
        FolderImages = 3,
        [StringValue("Memorandums\\")]
        FolderUpdates = 4,
        [StringValue("review\\")]
        FolderReview = 5,
        [StringValue("docs\\")]
        FolderDocs = 6,
        [StringValue("newsletters\\")]
        FolderNews = 7,
        [StringValue("videos\\")]
        FolderVideos = 8,
        [StringValue("events\\")]
        FolderEvents = 9,
        [StringValue("phoenix\\")]
        FolderPhoenix = 10,
        [StringValue("Justificaciones\\")]
        FolderJustificaciones = 11,
    }
    public enum EnumxBackofficePathToReadFile
    {
        [StringValue("~/src/images/")]
        Image = 1,
    }
    public enum EnumRowGrid
    {
        [StringValue("")]
        Limit = 2000,
    }
    public enum EnumTypeRate
    {
        [StringValue("")]
        ExchangeRate = 1,
        [StringValue("")]
        PegRate = 2,
    }
    public enum EnumFolderType
    {
        [StringValue("products\\")]
        FolderProducts = 1,
        [StringValue("profile\\")]
        FolderProfile
    }

    public enum EnumResourcesExtlName
    {

        [StringValue("_profile.jpg")]
        Profile = 1,
        [StringValue("_product.jpg")]
        Product = 2,
        [StringValue("_mainhome.jpg")]
        MainHome = 3,
        [StringValue("_lefthome.jpg")]
        LeftHome = 4,
        [StringValue("_centerhome.jpg")]
        CenterHome = 5,
        [StringValue("_rigthhome.jpg")]
        RigthHome = 6,
        [StringValue("_cover.jpg")]
        Cover = 7,
        [StringValue("_background.jpg")]
        Background = 8,
        [StringValue("_aboutComp.jpg")]
        AboutComp = 9,
        [StringValue("_contact.jpg")]
        Contact = 10,
        [StringValue("_complan.pdf")]
        Complan = 11
    }

    public enum EnumAddressType
    {
        Home = 0,
        Billing = 1,
        Shipping = 2,
        Work = 3,
        Taxpayer = 4,
        Residence = 5,
        Mailing = 6,
        Contact = 10
    }

    public enum EnumAddresPayment
    {
        [StringValue("Billing")]
        Billing = 1,
        [StringValue("Shipping")]
        Shipping = 2,
        [StringValue("Other")]
        Other = 0
    }

    public enum EnumDefaultCard
    {
        [StringValue("Default")]
        Default = 0,
        [StringValue("Backup")]
        Backup = 1
    }

    public enum EnumAccountType
    {
        [StringValue("Distributor")]
        Distributor = 10,
        [StringValue("Preferred Customer")]
        PreferredCustomer = 20,
        [StringValue("Retail Customer")]
        RetailCustomer = 30,
        [StringValue("Retail")]
        Retailer = 40


    }


    //public enum EnumAccountType
    //{
    //    PreferredCustomer = 0,  // Enroll
    //    IndependentExecutive = 1,
    //    RetailCustomer = 2 //para Buy
    //}
    public enum EnumBankAccountType
    {
        Checking = 0,
        Savings = 1
    }

    public enum EnumDomain
    {
        [StringValue("www.tru-friends.com")]
        Domain = 1,
        [StringValue("test.tru-friends.com")]
        Test = 2,
        [StringValue("localhost")]
        Localhost = 3

    }

    //public enum EnumCloseAccount
    //{
    //    [StringValue("I chose a different solution")]
    //    DifferenSolution = 1,
    //    [StringValue("The pricing is confusing")]
    //    PricingConfusing = 2,
    //    [StringValue("The pricing is too high")]
    //    PricingHigh = 3,
    //    [StringValue("The product is too difficult to use")]
    //    TheProductDifficult = 4,
    //    [StringValue("I do not host events")]
    //    Localhost = 5,
    //    [StringValue("The product lacks the necessary features")]
    //    Localhost = 6,
    //    [StringValue("I do not recall signing up for xEvent")]
    //    Localhost = 7,
    //    [StringValue("Other (Please explain)")]
    //    Localhost = 8,


    //}

    public enum EnumDomainReplicate
    {
        [StringValue("xreplicate-asea.xirectss.com")]
        Domain = 1,
        [StringValue("xreplicate-asea.xirectss.com")]
        Test = 2,
        [StringValue("localhost")]
        Localhost = 3
    }

    public enum EnumAppType
    {
        [StringValue("Free")]
        Free = 1,
        [StringValue("Paid")]
        Paid = 2
    }
    public enum EnumLogType
    {
        [StringValue("Product")]
        Product = 1,
        [StringValue("User")]
        User = 2,
        [StringValue("Parties")]
        Parties = 20,
    }
    public enum EnumxReplicatePathToReadFile
    {
        [StringValue("/src/distributor/images/profile/")]
        Image = 1
    }
    public enum EnumActionEmailTemplate
    {
        [StringValue("All")]
        All = -1,
        [StringValue("User")]
        User = 0,
        [StringValue("System")]
        System = 1,
        [StringValue("Invitation")]
        Invitation = 2,
        [StringValue("Remind")]
        Remind = 3,
        [StringValue("Gratitude")]
        Gratitude = 4,
        [StringValue("Missyou")]
        Missyou = 5
    }

    public enum EnumReportTypes
    {
        [StringValue("SP_XB_RPT_EXECUTIVEDASHBOARD")]
        ExecutiveDashboard = 1,

        [StringValue("SP_XB_RPT_COMMISSIONSUMMARY")]
        CommissionSummary = 2,

        [StringValue("SP_XB_RPT_ORDERHISTORY")]
        OrderHistory = 3,

        [StringValue("SP_XB_RPT_EARNINGSHISTORY")]
        EarningsHistory = 4,

        [StringValue("SP_XB_REPORT_DistributorsbyStatus")]//No existe SP
        DistributorsbyStatus = 5,

        [StringValue("SP_XB_REPORT_Autoships")]//No existe SP
        Autoships = 6,
        //fhGgAQNK31qtvtVAGJcnfQ%3d%3d
        [StringValue("SP_XB_REPORT_Birthdays")]//No existe SP
        Birthdays = 11,

        [StringValue("SP_XB_REPORT_CustomersbyStatus")]//No existe SP
        CustomersbyStatus = 8,

        [StringValue("SP_XB_REPORT_DistributorsBirthdays")]//No existe SP
        DistributorsBirthdays = 9,


        [StringValue("SP_REPORT_DISTRIBUTORPURCHASES")]
        TotalPurchases = 17,
        //TcSdlvGa0dp%2fGAKz3V7AQA%3d%3d

        [StringValue("SP_REPORT_TOTALBONUSESBYPERIOD")]//
        TotalBonusesForThisPeriod = 15,
        //jOwa2VF0Un9QaXSixX%2bhTQ%3d%3d

        [StringValue("SP_XB_REPORT_DistributorsBirthdays")]//No existe SP
        TotalCurrentInventory = 12,
        //XoTKnBwxiwBPqPv7WpgMvQ%3d%3d

        [StringValue("SP_REPORT_TOTALSALES_BYDISTRIBUTORID")]//
        TotalSales = 13,
        //D8XqVnsEtlqDMV13azxvmA%3d%3d

        [StringValue("SP_XB_REPORT_DistributorsBirthdays")]
        TotalGroupPurchases = 16,
        //S9f1%2fWAl5%2fAU63nSqBjcyw%3d%3d
        [StringValue("SP_XB_REPORT_OrdersByFriendsdates")]
        OrdersByFriendsdates = 7,
        //%2fifyS9R9AXIuXsOSXOSTEg%3d%3d
        [StringValue("SP_XB_REPORT_TotalPayoutstoFriends")]
        TotalPayoutstoFriends = 14,
        //%2bdXIjP4cm6nFedGiJB1kOg%3d%3d
        [StringValue("SP_XB_REPORT_Recruiting")]
        Recruiting = 10
        //CEXIKfZeDy9fnCDI9ClBeQ%3d%3d
    }

    public enum EnumDistributorPlacementLeg
    {
        [StringValue("Balance")]
        Balance = 0,
        [StringValue("Left Leg")]
        Left_Leg = 1,
        [StringValue("Right Leg")]
        Right_Leg = 2
    }

    public enum EnumSettingsNFRGlobal
    {
        [StringValue("Placement Leg")]
        Placement_Leg = 1
    }

    public enum EnumDistributorStatus
    {
        [StringValue("Begun Enrollment")]
        BegunEnrollment = 0,
        [StringValue("Cancelled")]
        Cancelled = 1,
        [StringValue("Active")]
        Active = 2,
        [StringValue("Inactive")]
        Inactive = 3,
        [StringValue("Suspended")]
        Suspended = 4,
        [StringValue("Deleted")]
        Deleted = 5,
        [StringValue("Terminate")]
        Terminate = 6,
        [StringValue("Lock xBackOffice")]
        LockxBacOffice = 7,
        [StringValue("Force Active")]
        ForceActive = 8,
        [StringValue("Override Qualifications")]
        OverrideQualifications = 9,
        [StringValue("Tax Exempt")]
        TaxExempt = 10,
        [StringValue("Synchronized")]
        Synchronized = 11,
        [StringValue("All")]
        All = -1,
    }


    public enum EnumDistributorStatus_Infotrax
    {
        [StringValue("C")]
        [StringMessage("Retail Customer")]
        RetailCustomer = 10,

        [StringValue("D")]
        [StringMessage("Associate")]
        Associate = 20,

        [StringValue("E")]
        [StringMessage("Employee")]
        Employee = 30,

        [StringValue("N")]
        [StringMessage("Temp Account Type")]
        Temp = 40,

        [StringValue("P")]
        [StringMessage("Preferred Customer")]
        PreferredCustomer = 50,

        [StringValue("S")]
        [StringMessage("Suspended")]
        Suspended = 60,

        [StringValue("T")]
        [StringMessage("Terminated")]
        Terminated = 70,

        [StringValue("B")]
        [StringMessage("Buy Now Customer")]
        BuyNowCustomer = 80,

        [StringValue("V")]
        [StringMessage("Buy Now Sponsors")]
        BuyNowSponsors = 90
    }

    public enum EnumOrganizerStatus
    {
        [StringValue("Cancelled")]
        Cancelled = 1,
        [StringValue("Active")]
        Active = 2,
        [StringValue("Inactive")]
        Inactive = 3,
        [StringValue("Suspended")]
        Suspended = 4,
        [StringValue("Deleted")]
        Deleted = 5,
      
    }

    public enum EnumUserStatus
    {
        Inactive = 0,
        Active = 1,
        Deleted = 2,
        Cancelled = 3,
        Suspended = 4,
        Blocked = 5
    }

    public enum EnumMyApp
    {
        [StringValue("TruDating")]
        TruDating = 1
    }

    public enum EnumOperationType
    {
        [StringValue("Email Sent")]
        EmailSent = 0,
        [StringValue("Share Contact")]
        ShareContacts = 1,
        [StringValue("Campaign Sent")]
        CampaignSent = 2
    }

    public enum EnumModules //Name Site
    {

        [StringValue("TruFriends")]
        TruFriends = 1,
        [StringValue("TruFriendsTest")]
        TruFriendsTest = 2
    }

    public enum EnumThemes
    {
        [StringValue("classic")]
        classic = 1,
        [StringValue("black")]
        black = 2
    }

    public enum EnumTypeEvent
    {
        [StringValue("Challenge")]
        Challenge = 11,
        [StringValue("Competition")]
        Competition = 13,
        [StringValue("Contest")]
        Contest = 14
    }

    public enum EventEndIntructionsType
    {
        ArchiveEvent = 1,
        OneTimeEvent = 0
    }


    public enum EnumDefault
    {
        [StringValue("RoundsNumber")]
        RoundsNumber = 3
    }

    public enum EnumVotingTypes
    {
        [StringValue("Judges Panel")]
        JudgesPanel = 1,
        [StringValue("Online Voting")]
        OnlineVoting = 2
    }

    public enum EnumVotingRestriccion
    {
        [StringValue("By Ip")]
        ByIp = 19,
        [StringValue("Hour")]
        ByHour = 20,
        [StringValue("Day")]
        ByDay = 21,
        [StringValue("Month")]
        ByMonth = 22,
        [StringValue("Event")]
        ByEvent = 23
    }

    public enum EnumCategoryTypes
    {
        [StringValue("Currency")]
        Currency = 1,
        [StringValue("Award")]
        Award = 2,
        [StringValue("Voting Types")]
        VotingType = 3,
        [StringValue("Voting Restriction")]
        VotingRestriction = 4,
        [StringValue("TypeTeam")]
        EventMember = 5,
        [StringValue("TypeSatff")]
        TypeStaff = 7,
        [StringValue("RelatedEventCategory")]
        RelatedEventCategory = 10,
        [StringValue("EventCoverage")]
        EventCoverage = 11
    }

    public enum EnumTypeEventMember
    {
        [StringValue("ManagedTeam")]
        ManagedTeam = 58,
        [StringValue("CoorporateStaff")]
        CoorporateStaff = 59,
        [StringValue("Sponsors")]
        Sponsors = 60,
        [StringValue("Contestants")]
        Contestants = 75,
        [StringValue("Judges")]
        Judges = 78
    }

    public enum EnumTypeStaff
    {
        [StringValue("Employees")]
        Employees = 68,
        [StringValue("Investors")]
        Investors = 69,
        [StringValue("Joint Venture Partners")]
        JointVenturePartners = 70,
        [StringValue("Member")]
        Member = 74
    }

    public enum EnumGenealogyIco
    {
        
        [StringValue("circle")]
        circle = 0,
        [StringValue("star")]
        star = 1,
        [StringValue("point")]
        point = 2,
        [StringValue("circle")]
        circle2 = 3
    }
    public enum EnumGenealogyLimit
    {

        [StringValue("BackupMax")]
        BackupMax =Int32.MaxValue,
       
    }
    public enum EnumGenealogyStatus
    {

        [StringValue("completed")]
        complete = 10,
        [StringValue("scheduled")]
        scheduled = 20,
        [StringValue("failed")]
        failed = 30,
        [StringValue("cancelled")]
        cancelled = 40,
        [StringValue("nonautoship")]
        nonautoship = 60,
        [StringValue("nonqualautoship")]
        nonqualautoship = 70
        
    }
    public enum EnumGenealogyStatusWeb
    {

        [StringValue("completed")]
        complete = 10,
        [StringValue("scheduled")]
        scheduled = 20,
        [StringValue("Failed")]
        failed = 30,
        [StringValue("cancelled")]
        cancelled = 40,
        [StringValue("nonautoship")]
        nonautoship = 60,
        [StringValue("nonqualautoship")]
        nonqualautoship = 70

    }

    public enum EnumRegistrationToEnterEvent
    {
        Paid = 1,
        Free = 0
    }
    public enum EnumEventViewing
    {
        Private = 1,
        Public = 0
    }
    public enum EnumSettingsType
    {
        Comments = 1,
        Thumbs = 2,
        Stars = 3
    }
    public enum EnumCoreForm
    {

         [StringMessage("?rl={3}&ur={0}&dt={1}&m={2}")]
         [StringValue("Join")]
        Join = 1,
         [StringMessage("?rl={3}&ur={0}&dt={1}&m={2}")]
         [StringValue("Order")]
        Order = 2,
        [StringMessage("?rl={4}&ur={0}&q={1}&dt={2}&m={3}")]
         [StringValue("Orderedit")]
         Orderedit = 3
    }
    public enum EnumSettingsOption
    {
        Enable = 1,
        Disable = 0
    }
    public enum EnumVirtualPath //Path para el APP virtual
    {
        [StringValue("")]
        xBackOffice = 0,
        [StringValue(@"C:\FolderPages\MySite")]
        xReplicate = 1,
        [StringValue("")]
        xCorporateWeb = 2,
        [StringValue(@"C:\FolderPagesTest\MySite")]
        xReplicateTest = 3,
        [StringValue(@"D:\FolderPages\TruFriends\MySite")]
        MySite = 4,
        [StringValue(@"D:\FolderPages\TruFriendsTest\MySite")]
        MySiteTest = 5
    }
    public enum EnumAppPool
    {
        [StringValue("TruFriends")]
        TruFriends = 0,
        [StringValue("TruFriendsTest")]
        TruFriendsTest = 1

    }
    public enum EnumOrderDetailType
    {
        [StringValue("Product")]
        Product = 0,
        [StringValue("App")]
        App = 1

    }
    public enum EnumAppDomainAppId
    {
        [StringValue("pADducIiXlXhKmgqMPvZAUi2J7oPTTQxJ6QQWT1NHyTiAhxsvts0bAxLse7n")]
        xCorporate = 0

    }

    public enum EnumSQLTables
    {
        [StringValue("TBL_TF_EVENT")]
        TBL_TF_EVENT = 1

        //[StringValue("App")]
        //App = 1

    }

    public enum EnumTypeLog
    {
        [StringValue("Propay")]
        Propay = 1,
        [StringValue("HyperWalletPay")]
        HyperWalletPay = 3,
        [StringValue("HyperWalletCreateAccount")]
        HyperWalletCreateAccount = 4,
        [StringValue("DistributorJoinDateUpdate")]
        DistributorJoinDateUpdate = 5
    }

    public enum EnumRelatedCategory
    {
        [StringValue("Innovation")]
        Innovation = 79,
        [StringValue("Investments")]
        Investments = 80,
        [StringValue("Technology")]
        Technology = 81,
        [StringValue("Products")]
        Products = 82,
        [StringValue("Services")]
        Services = 83,
        [StringValue("Processes_Methods")]
        Processes_Methods = 84,
        [StringValue("Entertainment")]
        Entertainment = 85,
        [StringValue("Music")]
        Music = 86,
        [StringValue("Movie_Video")]
        Movie_Video = 87,
        [StringValue("Art")]
        Art = 88,
        [StringValue("Community")]
        Community = 89,
        [StringValue("Family")]
        Family = 90,
        [StringValue("Company")]
        Company = 91,
        [StringValue("Non_Profit")]
        Non_Profit = 92,
        [StringValue("Government")]
        Government = 93
    }

    public enum EnumTypeReview
    {
        STORY = 1,
        PHOTO = 2,
        ABOUTCOMPANY = 3,
        MISSION = 4,
        CONTACT = 5,
        CORPMESSAGE = 6,
        COMPLAN = 7,
        HOMEIMGMAIN = 8,
        HOMEIMGLEFT = 9,
        HOMEIMGCENTER = 10,
        HOMEIMGRIGHT = 11,
        ABOUTCOMPANYPICTURE = 12,
        CONTACTPICTURE = 13
    }

    public enum EnumEventType
    { 
        Global = 1,
        Distributor = 2,
        Online=3
    }

    public enum EnumEventLocationType
    {
        [StringValue("Virtual Meeting")]
        Virtual_Meeting = 1,
        [StringValue("Physical")]
        Physical = 2
    }

    public enum EnumEventCoverage
    {
        [StringValue("Local - District or City")]
        Local_District_City = 94,
        [StringValue("State - Province")]
        State_Province = 95,
        [StringValue("Regional")]
        Regional = 96,
        [StringValue("National")]
        National = 97,
        [StringValue("International")]
        International = 98,
        [StringValue("Global")]
        Global = 99
    }

    public enum EnumEventLasting
    {
        [StringValue("Day")]
        Days = 0,
        [StringValue("Week")]
        Weeks = 1,
        [StringValue("Month")]
        Months = 2
    }


    public enum EnumEventStatus
    {
        [StringValue("Cancelled")]
        Cancelled = 1,
        [StringValue("Active")]
        Active = 2,
        [StringValue("Deleted")]
        Published = 3,
        [StringValue("Created")]
        Inactive = 4,
        [StringValue("Suspended")]
        Suspended = 5,
        [StringValue("Deleted")]
        Deleted = 6,
        [StringValue("Running")]
        Running = 7,
        [StringValue("Expired")]
        Expired = 8,
    }

    public enum EnumFrequency
    {
        [StringValue("Weekly")]
        Weekly = 0,
        [StringValue("Bi-Weekly")]
        BiWeekly = 1,
        [StringValue("Monthly")]
        Monthly = 2
    }
    public enum EnumQuerystring
    {
        [StringValue("aXis.Corporate.|{0}|.Orders")]
        AxisCorporateOrders = 0,
        [StringValue("aXis.Corporate.|{0}|.Distributor")]
        AxisCorporateDistributor = 1,
        [StringValue("aXis.Corporate.|{0}|.Autoship")]
        AxisCorporateAutoship = 2,
        [StringValue("aXis.Corporate.|{0}|.AdjusmentsOverrides")]
        AxisCorporateAdjusmentsOverrides = 3
    }

    public enum EnumAlertType
    {
        [StringValue("s")]
        Success = 1,
        [StringValue("e")]
        Error = 2,
        [StringValue("i")]
        Info = 3,
        [StringValue("c")]
        Confirm = 4
    }
    public enum TypeSheet
    {
        [StringValue("Empleado")]
        Empleado = 0,
        [StringValue("Obrero")]
        Obrero = 1
    }
    public enum Riesgo
    {
        [StringValue("Si")]
        Si= 1,
        [StringValue("No")]
        No = 0
    }
    public enum AddressType 
    {

        Home = 0,
        Billing = 1,
        Shipping = 2,
        Mailing = 3,
        Contact = 10

    }
    public enum EnumEnvironment
    {
 
        [StringMessage("lbldevenvironment")]
        [StringValue("Dev Environment")]
        Dev = 1,
        [StringMessage("lbltestenvironment")]
        [StringValue("Test Environment")]
        Test = 2,
        [StringMessage("lblstageenvironment")]
        [StringValue("Stage Environment")]
        Stage = 3,
        [StringMessage("")]
        [StringValue("")]
        Live = 4
    }

    public enum EnumFeeType
    {
        Money = 1,
        Percent = 2
    }

    public enum EnumRegex
    {
        [StringMessage("Error Required {0}")]
        [StringValue(@"^*$")]
        None = 0,
        [StringMessage("Error Phone {0}")]
        [StringValue(@"^([\+][0-9]{1,3}[ \.\-])?([\(]{1}[0-9]{2,6}[\)])?([0-9 \.\-\/]{3,20})((x|ext|extension)[ ]?[0-9]{1,4})?$")]
        Phone = 1,
        [StringMessage("Error in the email format")]
        [StringValue(@"^((([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$")]
        Email = 2,
        [StringMessage("Error Integer {0}")]
        [StringValue(@"^[\-\+]?\d+$")]
        Integer = 3,
        [StringMessage("Error Number {0}")]
        [StringValue(@"^[\-\+]?(?:\d+|\d{1,3})(?:\.\d+)$")] // Number, including positive, negative, and floating decimal.
        Number = 4,
        [StringMessage("Error Date {0}")]
        [StringValue(@"^\d{1,2}[\/\-]\d{1,2}[\/\-]\d{4}$")]
        Date = 5,
        [StringMessage("Error CreditNumber {0}")]
        [StringValue(@"^([0-9][0-9]{0,4})+([0-9][0-9]{0,4})+([0-9][0-9]{0,4})+([0-9][0-9]{0,4})+$")]        
        CreditNumber = 6,
        [StringMessage("Error URL {0}")]
        [StringValue(@"^(http(?:s)?\:\/\/[a-zA-Z0-9\-]+(?:\.[a-zA-Z0-9\-]+)*\.[a-zA-Z]{2,6}(?:\/?|(?:\/[\w\-]+)*)(?:\/?|\/\w+\.[a-zA-Z]{2,4}(?:\?[\w]+\=[\w\-]+)?)?(?:\&[\w]+\=[\w\-]+)*)$")]
        URL = 7,
        [StringMessage("Error DateHour {0}")]
        [StringValue(@"^\d{1,2}[\/\-]\d{1,2}[\/\-]\d{4}\s([0-9][0-9]{0,2}):([0-9][0-9]{0,2})$")]        
        DateHour = 8,
        [StringMessage("Error OnlyNumber {0}")]
        [StringValue(@"^[0-9\ ]+$")]
        NumberAndSpaces = 9,
        [StringMessage("Error NoSpecialCharacters {0}")]
        [StringValue(@"^[0-9a-zA-Z \']+$")]
        NoSpecialCharacters = 10,
        [StringMessage("Error OnlyLetter {0}")]
        [StringValue(@"^[a-zA-Z\ \']+$")]
        OnlyLetter = 11,
        [StringMessage("Error Only Number and 2 Decimal {0}")]
        [StringValue(@"^\d+(\.\d{1,2})?$")]        
        NumberAnd2Decimal = 12,
        [StringMessage("Error Only Number and 3 Decimal {0}")]
        [StringValue(@"^\d+(\.\d{1,3})?$")]
        NumberAnd3Decimal = 13,
        [StringMessage("Error OnlyNumber {0}")]
        [StringValue(@"^[0-9]+$")]
        OnlyNumber = 14,

        [StringMessage("Error Name {0}")] //ejemplo
        [StringValue(@"^[0-9]+$")]
        Name = 15,
        [StringMessage("Error Date {0}")]
        [StringValue(@"^\d{4}[\-]\d{2}[\-]\d{2}$")]
        Date_YYYYMMDD = 16,
        [StringMessage("Error NoSpecialCharacters {0}")]
        [StringValue(@"^[a-zA-Z]{2}$")]
        ISO_2code = 17,
        [StringMessage("Error Date {0}")]
        [StringValue(@"^[0-9]{12}$")]
        Date_YYYYMMDDHHmm = 18,
        [StringMessage("Error LettersHyphens {0}")]
        [StringValue(@"^[A-Za-z\ \d_-]+$")]
        LettersHyphens = 19,
        [StringMessage("Error LettersNumberHyphens {0}")]
        [StringValue(@"^[0-9a-zA-Z\d_-]+$")]
        LettersNumberHyphens = 20,
        [StringMessage("Error Format Zip {0}")]
        [StringValue(@"^\d{5}(?:[\s-]\d{4})?$")]
        Zip_usa = 21,
        [StringMessage("Error Only Letters and Numbers {0}")]
        [StringValue(@"^[0-9a-zA-Z]+$")]
        Letter_numbers = 22,
        [StringMessage("Error Format Zip {0}")]
        [StringValue(@"^[A-Z]{1}\d{1}[A-Z]{1} ?\d{1}[A-Z]{1}\d{1}$")]
        Zip_canada = 23,
        [StringMessage("Error in the email format")]
        [StringValue(@"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$")]
        CorrectEmail = 24,
    }
    public enum EnumRegexLength
    {        
        None = 0,        
        Exact = 1,        
        AtLeast = 2,        
        Between = 3,        
        Max = 4
    }

    public enum EnumStatus
    {
        [StringValue("n")]
        Disabled = 0,//Inactive
        [StringValue("y")]
        Enabled = 1,//Active
        Deleted = 2,
        ShowAll = 3,
        [StringValue("s")]
        Suspended = 4,//Suspended
        [StringValue("u")]
        notSuspended = 5,
        [StringValue("p")]
        purged = 6
    }

    public enum EnumDisabled 
    {
        [StringValue("n")]
        Disabled = 1,//Inactive
        [StringValue("y")]
        Enabled = 0,//Active
        Deleted = 2
    }

    public enum EnumLanguage
    { 
        [StringValue("en-US")]
        United_States = 1,
        [StringValue("es-ES")]
        Spain = 2,
        [StringValue("fr-CA")]
        Français_Canadien = 3,
        [StringValue("de-DE")]
        German = 4,
        [StringValue("hu-HU")]
        Hungary = 5,
        [StringValue("fr-FR")]
        France = 6,
        [StringValue("en-IE")]
        Ireland = 7,
        [StringValue("nl-NL")]
        Netherlands = 8,
        [StringValue("de-AT")]
        Austria = 9,
        [StringValue("it-IT")]
        Italy = 10,
        [StringValue("en-GB")]
        Great_Britain = 11,
        [StringValue("sl-SI")]
        Slovenia = 12,
        [StringValue("nn-NO")]
        Norway = 13,
        [StringValue("nl")]
        Dutch = 14,
        [StringValue("fr-BE")]
        French_Belgium = 15,
        [StringValue("nl-BE")]
        Dutch_Belgium = 16,
        [StringValue("sv-SE")]
        Swedish = 17,
        [StringValue("hr-HR")]
        Croatian = 18,
        [StringValue("da-DK")]
        Denmark = 19,
        [StringValue("ro")]
        Romanian = 20,
        [StringValue("es-MX")]
        Mexico_Spanish = 21,
        [StringValue("es-US")]
        UnitedStates_Spanish = 29,
        /*[StringValue("es-PE")]
        Peru = 3*/
    }

    /*public enum EnumResxFilesUrl
    {
        //[StringValue(@"D:\Axis2\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")]
        [StringValue(@"C:\Users\Eder Salinas\Documents\Tortoise\Axis2.0\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")]//EDER HOME!!
        //[StringValue(@"C:\Users\Eder Salinas\Documents\Tortoise\Axis2\trunk\xReplicate - StyleFinal\xCorporate\App_GlobalResources\Strings.")]//EDER HOME!!
        //[StringValue(@"D:\xAPPS\xReplicate\App_GlobalResources\Strings.")]
        //[StringValue(@"D:\Axis2\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")]
        //[StringValue(@"D:\Subversion\Axis2.0\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")]
        //[StringValue(@"C:\Users\Richard\Documents\XIRECTSS\axis2.0\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")] 
        //[StringValue(@"D:\aXis2.0\trunk\xCorporate\xCorporate\App_GlobalResources\Strings.")]                
        Resx = 1
    }*/

    public enum EnumCachesName
    { 
        [StringValue("Nav")]
        MenuTabs = 1,
        [StringValue("Rng")]
        RangeIp = 2,
        [StringValue("Lng")]
        Languages = 3,
        [StringValue("Acc")]
        CountryAccess = 4
    }

    public enum EnumKeysLanguageCache
    {
        [StringValue("All")]
        All = 1
    }

    public enum EnumKeysRangeIPCache
    {
        [StringValue("All")]
        All = 1
    }

    public enum EnumKeysCountryAccessCache
    {
        [StringValue("All")]
        All = 1
    }

    public enum EnumCountries//TBL_TF_COUNTRIES
    {
        [StringValue("United States")]
        United_States = 55
    }

    public enum EnumIPDefault
    {
        [StringValue("100.0.0.1")]
        IPDefaulUSA = 1
    }

    public enum EnumTypeAplication
    {
        [StringValue("All")]
        All = -1,
        [StringValue("Backoffice")]
        Axis = 1,
        [StringValue("Replicate-ASEA")]
        Replicate_Asea = 2
    }
    public enum EnumOrderType
    {
        [StringValue("AutoShip")]
        Autoship = 106,
        [StringValue("XPP")]
        Orders = 107,
        [StringValue("Enrrollment")]
        Enrrollment = 108,
        [StringValue("Party")]
        Party = 109,
    }

    public enum EnumRecognitionType
    {
        [StringValue("ECARDS")]
        Ecards = 1,
        [StringValue("Autoresponders")]
        Autoresponders = 2
    }

    public enum EnumAppCode
    {
        [StringValue("All")]
        All = -1,
        [StringValue("ASEA")]
        ASEA = 1,
        [StringValue("xBackOffice-ASEA")]
        xBackOffice_ASEA = 2,
        [StringValue("xCorporate")]
        xCorporate = 3,
        [StringValue("ResourcesManagement")]
        ResourcesManagement = 4,
        [StringValue("MyPhotos")]
        MyPhotos = 5,
        [StringValue("GoalsManagement")]
        GoalsManagement = 6,
        [StringValue("MyProductPhotos")]
        MyProductPhotos = 7,
        [StringValue("ASEA_v3")]
        ASEA_v3 = 10,
        [StringValue("PartyPlan")]
        PartyPlan = 11,
        [StringValue("Soxial")]
        SoxialMarketing = 12,
        [StringValue("VO2016")]
        Vo2016 = 13,
        [StringValue("Phoenix")]
        Phoenix = 14
    }

    public enum EnumAction
    { 
        Insert = 1,
        Update = 2,
        Delete = 3,
        Select = 4
    }

    public enum EnumTypeContact
    { 
        ContactMe = 2
    }

    public enum EnumEmailApp
    {
        [StringValue("Contactme_xreplicated mail logs")]
        Contactme_xreplicted = 1,
        [StringValue("Axis")]
        Axis = 2
    }

    public enum EnumReportException
    {
        [StringValue("All")]
        All = 1,
        [StringValue("Not Found in XSS")]
        NotFoundXSS = 2,
        [StringValue("Not Found in InfoTrax")]
        NotFoundInfoTrax = 3,
        [StringValue("Business Site Name are different")]
        BusinessSitedifferent = 4

    }




    
    public enum EnumUCLoadGrid
    {
        [StringValue("LoadGrid")]
        LoadGrid = 0,
        [StringValue("LoadGridProd")]
        LoadGridProd = 1
    }

    public enum EnumImageTypeOnEventRegistration
    {
        [StringValue("BAR")]
        BC = 1,
        [StringValue("QR")]
        QR = 2
    }
    public enum EnumUserType
    {
        [StringValue("User")]
        User = 0,
        [StringValue("Role")]
        Role = 1
    }


    public enum EnumResourceType
    {
        Audio = 1,
        Image = 2,
        IdentificationDocument = 3,
        Photos=4

    }

    //,
    //    Presentation = 4,
    //    WeminarLink = 5,
    //    Tutorial = 6


    public enum EnumVideoFileFormat
    {
        avi,
        mp4,
        wmv,
        fla,
        flv,
        mpeg,
        mpg
    }
    public enum EnumPresentationFileFormat
    {
        ppt,
        pptx
    }
    public enum EnumImageFileFormat
    {
        gif,
        jpeg,
        jpg,
        tif,
        png,
        psd,
        ai,
        eps,
        bmp

    }
    public enum EnumAudioFileFormat
    {
        mp3,
        wma,
        wav,
        midi
    }

    public enum EnumDocumentFileFormat
    {
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pdf,
        pptx,
        txt,
        html
    }
        

    public enum EnumFileExtentions
    {
        mp3,
        mpeg,
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pdf,
        pptx,
        wav,
        wmv,
        avi,
        mp4,
        m4v,
        gif,
        jpeg,
        jpg,
        tif,
        png,
        url,
        zip,
        rar,
        tar
    }


    public enum EnumMailCategory
    {
        [StringValue("Inbox")]
        Inbox = 1,
        [StringValue("Sent")]
        Sent = 2,
        [StringValue("Trash")]
        Trash = 3,
    }
    public enum EnumMailStatus
    {
        
        [StringValue("Read")]
        Read = 1,
        [StringValue("Unread")]
        Unread = 0,
        [StringValue("Delete")]
        Delete = 3
    }

    public enum EnumAlertStatus
    {
        [StringValue("Read")]
        Read = 1,
        [StringValue("Unread")]
        Unread = 0,             
    }

    public enum EnumDistributorAdmin
    {
        ID = 10
    }

    //Modify by Eder - xEvent
    public enum EnumGeneralStatus
    {
        [StringValue("Disabled")]
        Disabled = 0,
        [StringValue("Enabled")]
        Enabled = 1,
        [StringValue("Deleted")]
        Deleted = 2
    }

    // INI - Add by Dennys Távara - 15/12/2015
    public enum EnumMethodTypes
    {
        [StringValue("Create")]
        Create = 1,
        [StringValue("Update")]
        Update = 2,
        [StringValue("Get")]
        Get = 3,
        [StringValue("Delete")]
        Delete = 4
    }

    public enum EnumDataTypes
    {
        [StringValue("String")]
        String = 1,
        [StringValue("Int")]
        Int = 2,
        [StringValue("Boolean")]
        Boolean = 3,
        [StringValue("DateTime")]
        DateTime = 4,
        [StringValue("Decimal")]
        Decimal = 5,
        [StringValue("Custom")]
        Custom = 6
    }
    // FIN - Add by Dennys Távara - 15/12/2015


    //public enum EnumTaxType
    //{
    //    [StringValue("Individual/Sole Proprietor")]
    //    IndividualProprietor = 1,
    //    [StringValue("C Corporation")]
    //    CCorporation = 2,
    //    [StringValue("S Corporation")]
    //    SCorporation = 3,
    //    [StringValue("Partnership")]
    //    Partnership = 4,
    //    [StringValue("Trust/estate")]
    //    TrustEstate = 5,
    //    [StringValue("Limited liability company (C Corporation)")]
    //    LimitedCCorporation = 6,
    //    [StringValue("Limited liability company (S Corporation)")]
    //    LimitedSCorporation = 7,
    //    [StringValue("Limited liability company (Partnership)")]
    //    LimitedPartnership = 8,
    //    [StringValue("Exempt Payee")]
    //    ExemptPayee = 9,
    //    [StringValue("Other")]
    //    Other = 10
    //}



    //public enum EnumEntityType
    //{
    //    [StringValue("Grantor Trust")]
    //    GrantorTrust = 1,
    //    [StringValue("Central bank of issue")]
    //    CCorporation = 2,
    //    [StringValue("Individual")]
    //    SCorporation = 3,
    //    [StringValue("Complex trust")]
    //    Partnership = 4,
    //    [StringValue("Tax-exempt organization")]
    //    TrustEstate = 5,
    //    [StringValue("Corporation")]
    //    LimitedCCorporation = 6,
    //    [StringValue("Estate")]
    //    LimitedSCorporation = 7,
    //    [StringValue("Private foundation")]
    //    LimitedPartnership = 8,
    //    [StringValue("Disregarded entity")]
    //    ExemptPayee = 9,
    //    [StringValue("Government")]
    //    Other = 10,
    //    [StringValue("Partnership")]
    //    Other = 10,
    //    [StringValue("International organization")]
    //    Other = 10,
    //    [StringValue("Simple trust")]
    //    Other = 10

    //}


    public enum EnumInvoice
    {
        
        [StringValue("Pending")]
        Pending = 1,
        [StringValue("Paid")]
        Paid = 2
    }

    public enum EnumTaxPayerNumberType
    {
        Ssn = 1,
        Ein = 2,

    }

    public enum EnumTicketType 
    {
        [StringValue("Free")]
        Free = 0,
        [StringValue("Paid")]
        Paid = 1
    }

    public enum EnumCustomerType
    {
        [StringValue("Normal")]
        Normal = 0,
        [StringValue("Guest")]
        Guest = 1
    }


    public enum EnumAccInftrx
    {
        
        [StringValue("D")]
        [StringMessage("W")]
        Associate = 1,
        [StringValue("P")]
        [StringMessage("W")]
        PreferredCustomer = 2,
        [StringValue("C")]
        [StringMessage("R")]
        RetailCustomer = 3,

    }




    public enum EnumActionEmailTemplateParty
    {
        [StringValue("All")]
        All = -1,
        [StringValue("Invitation")]
        Invitation = 0,
        [StringValue("Remind")]
        Remind = 1,
        [StringValue("Gratitude")]
        Gratitude = 2,
        [StringValue("Missyou")]
        Missyou = 3
    }

    public enum EnumTemplateModule
    {
        [StringValue("Replicated Site")]
        ReplicatedSite = 2,
        [StringValue("Party Plan")]
        PartyPlan = 3,
        [StringValue("AutoResponders")]
        AutoResponder = 4,
        [StringValue("General")]
        General = 5,
         [StringValue("Recognition")]
        Recognition = 6
    }



    public enum EnumModule
    {
        [StringValue("xCorporate")]
        xCorporate = 1,
        [StringValue("xBackoffice")]
        xBackoffice = 2,
        [StringValue("xReplicate")]
        xReplicate = 3
    }

    public enum EnumAccType
    {
        [StringValue("Associate")]
        Associate = 10,
        [StringValue("Preferred Customer")]
        PreferredCustomer = 20,
        [StringValue("Retail Customer")]
        RetailCustomer = 30
        
    }
    public enum EnumPriceLevel
    {
        [StringValue("Wholesale")]
        Wholesale = 1,
        [StringValue("Retail")]
        Retail = 2,
        [StringValue("Preferred")]
        Preferred = 3

    }

        public enum EnumOrderFrom
        {
            [StringValue("xCorporate")]
            xCorporate = 1,
            [StringValue("xBackoffice")]
            xBackoffice = 2,
            [StringValue("xReplicate")]
            xReplicate = 3,
            [StringValue("xMobil")]
            xMobil = 4,
            [StringValue("xMerchant")]
            xMerchant = 5,
            [StringValue("xPP")]
            xPP = 6,
            [StringValue("Enroll")]
            xEnrollments = 7,
            [StringValue("Orders")]
            xOrders = 8,
            [StringValue("BuyNow")]
            xBuyNow = 9 ,
            [StringValue("NFR")]
            xNFR = 10,
            [StringValue("Autoships")]
            xAutoships = 11
        }

    public enum EnumMerchant
    {
        [StringValue("BrainTree")]
        BrainTree = 2, // IT MUST BE THE ID OF MERCHANT IN DATABASE
        [StringValue("GlobalCollect")]
        GlobalCollect = 3, // IT MUST BE THE ID OF MERCHANT IN DATABASE
        [StringValue("AuthorizeDotNet")]
        AuthorizeDotNet = 4, // IT MUST BE THE ID OF MERCHANT IN DATABASE
        [StringValue("BrainTreeBlue")]
        BrainTreeBlue = 4 // IT MUST BE THE ID OF MERCHANT IN DATABASE
    }

    public enum EnumJixiti_ApiKey
    {
        [StringValue("azgjhdf97321WFQ#$aGHFQ3$takhgfsavd")]
        ASEA = 1
    }

    public enum EnumPostAcion
    {
        [StringValue("Reach")]
        Reach = 10,
        [StringValue("Join")]
        Join = 20,
        [StringValue("Purchase")]
        Purchase = 30
    }


    public enum EnumBannerType
    {
        [StringValue("Report")]
        Report = 1,
        [StringValue("Recognition")]
        Recognition = 2
    }

    public enum EnumFilterCommission
    {
        [StringValue("Primary Bonus")]
        xPrimaryBonus=1,
        [StringValue("Secundary Bonus")]
        xSecundaryBonus = 2
    }

    public enum EnumRecognitionPage
    {
        [StringValue("Fanfare")]
        Fanfare = 1,
        [StringValue("Million_Dollar_Club")]
        MillionDollarClub = 2,
        [StringValue("Fantastic_Ahievements")]
        FantasticAhievements = 3,
        [StringValue("Base_Camp")]
        BaseCamp = 4,
        [StringValue("Ascent")]
        Ascent = 5,
        [StringValue("Envision")]
        Envision = 6,
        [StringValue("Diamond_Retreat")]
        DiamondRetreat = 7,
        [StringValue("Rank_Advancements")]
        RankAdvancements = 8,
        [StringValue("Peak_Performance")]
        Peak_Performance = 9
        
    }


    public enum EnumDistributorRanks
    {
        [StringMessage("")]
        [StringValue("Cancelled")]
        Cancelled = 0,
        [StringMessage("ASSOCIATE")]
        [StringValue("Associate")]
        Associate = 1,
        [StringMessage("DIRECTOR")]
        [StringValue("Director")]
        Director = 2,
        [StringMessage("DIRECTOR 300")]
        [StringValue("Director300")]
        Director300 = 3,
        [StringMessage("DIRECTOR 700")]
        [StringValue("Director700")]
        Director700 = 4,
        [StringMessage("BRONZE EXECUTIVE")]
        [StringValue("BronzeExecutive")]
        BronzeExecutive = 5,
        [StringMessage("SILVER")]
        [StringValue("SilverExecutive")]
        SilverExecutive = 6,
        [StringMessage("GOLD")]
        [StringValue("GoldExecutive")]
        GoldExecutive = 7,
        [StringMessage("PLATINUM")]
        [StringValue("PlatinumExecutive")]
        PlatinumExecutive = 8,
        [StringMessage("DIAMOND")]
        [StringValue("Diamond")]
        Diamond = 9,
        [StringMessage("DOUBLE DIAMOND")]
        [StringValue("DoubleDiamond")]
        DoubleDiamond = 10,
        [StringMessage("TRIPLE DIAMOND")]
        [StringValue("TripleDiamond")]
        TripleDiamond = 11
    }
    public enum EnumColorsRanks
    {
        [StringMessage("")]
        [StringValue("#E3E7E9")]
        Cancelled = 0,
        [StringMessage("")]
        [StringValue("#2774BA")]
        Associate = 1,
        [StringMessage("")]
        [StringValue("#2F8434")]
        Director = 2,
        [StringMessage("")]
        [StringValue("#44AB33")]
        Director300 = 3,
        [StringMessage("")]
        [StringValue("#ABD2A1")]
        Director700 = 4,
        [StringMessage("")]
        [StringValue("#D2914E")]
        BronzeExecutive = 5,
        [StringMessage("")]
        [StringValue("#AAB6C6")]
        SilverExecutive = 6,
        [StringMessage("")]
        [StringValue("#C5B257")]
        GoldExecutive = 7,
        [StringMessage("")]
        [StringValue("#7A8DA4")]
        PlatinumExecutive = 8,
        [StringMessage("")]
        [StringValue("#C871AB")]
        Diamond = 9,
        [StringMessage("")]
        [StringValue("#B5358B")]
        DoubleDiamond = 10,
        [StringMessage("")]
        [StringValue("#671B56")]
        TripleDiamond = 11
    }
    public enum EnumColorsRanksVo
    {
        [StringMessage("")]
        [StringValue("#E3E7E9")]
        Cancelled = 0,
        [StringMessage("")]
        [StringValue("#0C761B")]
        Associate = 1,
        [StringMessage("")]
        [StringValue("#1EBD34")]
        Director = 2,
        [StringMessage("")]
        [StringValue("#85D723")]
        Director300 = 3,
        [StringMessage("")]
        [StringValue("#BBD260")]
        Director700 = 4,
        [StringMessage("")]
        [StringValue("#E9F115")]
        BronzeExecutive = 5,
        [StringMessage("")]
        [StringValue("#CBA200")]
        SilverExecutive = 6,
        [StringMessage("")]
        [StringValue("#F17D00")]
        GoldExecutive = 7,
        [StringMessage("")]
        [StringValue("#AB462A")]
        PlatinumExecutive = 8,
        [StringMessage("")]
        [StringValue("#CD0C0C")]
        Diamond = 9,
        [StringMessage("")]
        [StringValue("#F30067")]
        DoubleDiamond = 10,
        [StringMessage("")]
        [StringValue("#CD0C9F")]
        TripleDiamond = 11
    }
    public enum EnumColorsVo
    {
        [StringMessage("")]
        [StringValue("#1EBD34")]
        IsNot = 0,
        [StringMessage("")]
        [StringValue("#0C761B")]
        Is = 1
    }
    public enum EnumShapeRanksVo
    {
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/circulo.png")]
        Associate = 1,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/triangleUp.png")]
        Director = 2,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/rectangulo.png")]
        Director300 = 3,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/ovalo1.png")]
        Director700 = 4,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/ovalo2.png")]
        BronzeExecutive = 5,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/estrella.png")]
        SilverExecutive = 6,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/rombo.png")]
        GoldExecutive = 7,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/pentagono.png")]
        PlatinumExecutive = 8,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/circulo2.png")]
        Diamond = 9,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/triangleLeft.png")]
        DoubleDiamond = 10,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/cuadrado.png")]
        TripleDiamond = 11
    }
    public enum EnumShapeVo
    {
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/triangleUp.png")]
        IsNot = 0,
        [StringMessage("")]
        [StringValue("../../src/vendor/jqtree/img/circulo.png")]
        Is = 1
    }

    public enum EnumColumnMessageVo
    {
        [StringMessage("")]
        [StringValue("")]
        White = 1,
        [StringMessage("")]
        [StringValue("[RenewalDate]")]
        RenewalDay = 2,
        [StringMessage("")]
        [StringValue("[HomeZip]")]
        PostalCode = 3,
        [StringMessage("")]
        [StringValue("[LifetimeRankId]")]
        PinkRank = 4,
        [StringMessage("")]
        [StringValue("[HomeState]")]
        State = 5,
        [StringMessage("")]
        [StringValue("[HomeCountry]")]
        Country = 6,
        [StringMessage("")]
        [StringValue("[TotalPV]")]
        PV = 7,
        [StringMessage("")]
        [StringValue("[Period_GV]")]
        GV = 8
    }

    public enum EnumFilterMessageVo
    {
        [StringMessage("")]
        [StringValue("")]
        EntireDownline = 1,
        [StringMessage("")]
        [StringValue("[TotalPV] > 0")]
        HasPersonalVolume = 2,
        [StringMessage("")]
        [StringValue("((SELECT TotalPV FROM TBL_XCOM_DISTRIBUTOR_COMMISSIONS where commperiodid "+
            "= (select max(commperiodid) from TBL_XCOM_DISTRIBUTOR_COMMISSIONS where legacynumber = d.legacynumber) and legacynumber = d.legacynumber)"+
            " > (SELECT totalpv  FROM TBL_XCOM_DISTRIBUTOR_COMMISSIONS where commperiodid = (select max(commperiodid-1) from TBL_XCOM_DISTRIBUTOR_COMMISSIONS "+
            "where legacynumber = d.legacynumber) and legacynumber = D.legacynumber))")]
        PVIncrease = 3,
        [StringMessage("")]
        [StringValue("[TotalPV] > [Period_GV]")]
        PVHigherThanGV = 4,
        [StringMessage("")]
        [StringValue("[EndRank] > 3")]
        Rank3AndAbove = 5,
        [StringMessage("")]
        [StringValue("[EndRank] > [StartRank]")]
        AnyoneThatHasAdvancedInRank = 6
    }

    public enum EnumFilter2MessageVo
    {
        [StringMessage("")]
        [StringValue("")]
        White = 0,
        [StringMessage("")]
        [StringValue("Between")]
        Between = 1,
        [StringMessage("")]
        [StringValue("Between")]
        Range = 2,
        [StringMessage("")]
        [StringValue(">=")]
        Since = 3,
        [StringMessage("")]
        [StringValue("=")]
        Equals = 4,
        [StringMessage("")]
        [StringValue(">=")]
        Minimum = 5,
        [StringMessage("")]
        [StringValue("<=")]
        Maximum = 6
    }

    public enum EnumFilterReport
    {
        [StringValue("Sponsorship")]
        xSponsorship = 0,
        [StringValue("Binary")]
        xBinary = 1

    }
    public enum EnumFilterEMP
    {
        [StringValue("Paid as Rank")]
        xPaidAsRank =0,
        [StringValue("Bonuses Paid")]
        xBonusesPaid =1
    }
    public enum EnumPeakPerformanceSubmenu
    {
        [StringValue("Envision")]
        Envision = 1,
        [StringValue("Ascent")]
        Ascent = 2,
        [StringValue("Diamond_Retreat")]
        Diamond_Retreat = 3,
        [StringValue("Base_Camp")]
        Base_Camp = 4,
        [StringValue("Peak_Performance")]
        Peak_Performance = 5
    }
    public enum EnumReportJixitiApi
    {
        [StringValue("Associates D Enrollees")]
        AssociatesD = 1,
        [StringValue("Associates B, C, P Enrollees")]
        AssociatesBCP = 2,
        [StringValue("Sponsor Changes")]
        SponsorChanges = 3,
        [StringValue("Title Changes")]
        TitleChanges = 4,
        [StringValue("Autoship Differences")]
        AutoshipDifferences = 5,
        [StringValue("Rank Changes")]
        RankChanges = 6,
        [StringValue("Activity Changes")]
        ActivityChanges = 7
    }
    public enum EnumReportTopTen
    {
        [StringValue("Top Income Earners")]
        TopIncomeEarners = 1,
        [StringValue("Top Associate Enrollers")]
        TopAssociateEnrollers = 2,
        [StringValue("Emerging Leaders")]
        EmergingLeaders = 3,
        [StringValue("Autoship All-Star")]
        AutoshipAllStar = 4
    }

    public enum EnumRecognitionTable
    {
        [StringValue("All")]
        All = 0,
        [StringValue("Fanfare")]
        Fanfare = 1,
        [StringValue("MillionDollarClub")]
        MillionDollarClub = 2,
        [StringValue("Top10Report")]
        Top10Report = 3,
        [StringValue("PeakPerformace")]
        PeakPerformace = 4,
        [StringValue("RankAdvancements")]
        RankAdvancements = 5
    }
    public enum EnumAssociateType
    {
        [StringValue("registered_business_nz")]
        registered_business_nz = 10,
        [StringValue("without_tax_id_nz")]
        without_tax_id_nz = 20,
        [StringValue("valid_tax_id_nz")]
        valid_tax_id_nz = 30,

        [StringValue("join_a_hobby_aus")]
        join_a_hobby_aus = 40,
        [StringValue("registered_business_aus")]
        registered_business_aus = 50
   
    }


    public enum EnumMarkets
    {
        [StringValue("United States")]
        UnitedStates = 7,
        [StringValue("Canada")]
        Canada = 22,
        [StringValue("New Zealand")]
        NewZealand = 53,
        [StringValue("Australia")]
        Australia = 45,
        [StringValue("Francia")]
        Francia = 25,
        [StringValue("Italia")]
        Italia = 29,
        [StringValue("Germany")]
        Germany = 23,
        [StringValue("Mexico")]
        Mexico = 52,
        [StringMessage("AUS")]
        [StringValue("Australia NFR")]
        AustraliaNFR = 58,
        [StringValue("Switzerland")]
        Switzerland = 57
    }

    public enum EnumLanguages
    {
        [StringValue("Deutsch (Deutschland)")]
        German = 4,
    
    }

    public enum EnumStatusPartyAtendee
    {
        Pending = 0,
        Confirmed = 1,
        Rejected = 2
    }
    public enum EnumEmailTemplatesType
    {
        Invited = 1,
        Missed = 2,
        Thank = 3,
        Invoice = 4,
        Review = 5

    }
    public enum EnumPartyConfirmation
    {
        [StringValue("Yes")]
        Yes = 1,
        [StringValue("No")]
        No = 2,
        [StringValue("Maybe")]
        Maybe = 3
    }

    public enum EnumEvalQuery
    {
        [StringValue("Enabled")]
        NoQuery = 0,
        [StringValue("Disabled")]
        OkQuery = 1,
        [StringValue("Deleted")]
        ErrorQuery = 2
    }


    public enum EnumAuth
    {
        [StringValue("Success")]
        NoRequired = 0,
        [StringValue("Required")]
        Required = 1
    }

    public enum EnumSettingsRegion
    {
        Company = 1,
        Commissions = 2,
        EmailConfig = 3,
        Notifications = 4,
        PaymentProcesing = 5,
        Markets = 6,
        Environment = 7,
        LegEnrollment = 8,
        ApiManagement = 9
    }


    public enum EnumPromoterSearchBy
    {
        [StringValue("Id")]
        Id = 0,
        [StringValue("LegacyNumber")]
        LegacyNumber = 1,
        [StringValue("Name")]
        Name = 2,
        [StringValue("LastName")]
        LastName = 3
    }


    public enum EnumPaymentType
    {
        [StringValue("CreditCard")]
        CreditCard = 1,
        [StringValue("Cash")]
        Cash = 2
    }

    public enum EnumBannerPosition
    {
        [StringValue("Main")]
        Main = 1,
        [StringValue("Center")]
        Center = 2,
        [StringValue("Footer1")]
        Footer1 = 3,
        [StringValue("Footer2")]
        Footer2 = 4
    }


    public enum EnumAutorepondersEnroll
    {
        [StringValue("ReceiveCorporateEmails")]
        Autoresponder1 = 1,
        [StringValue("ReceiveEmailsSponsor. ")]
        Autoresponder2 = 2,
        [StringValue("ReceiveCorporateCalls. ")]
        Autoresponder3 = 3,
        [StringValue("ReceiveCallSponsor.")]
        Autoresponder4 = 4
    }

    public enum EnumLanguageSession
    {
        [StringValue("en-US")]
        English_United_States = 1,
        [StringValue("es-US")]
        Spain_United_States = 2030,
        [StringValue("cs-CZ")]
        Cestina = 2024,
        [StringValue("da-DK")]
        Dansk = 19,
        [StringValue("de-DE")]
        Deutsch = 4,
        [StringValue("de-AT")]
        Deutsch_Osterreich = 9,
        [StringValue("en-AU")]
        English_Australia = 2026,
        [StringValue("en-CA")]
        English_Canada = 2022,
        [StringValue("da")]
        English_DenMark = 20,
        [StringValue("en-NZ")]
        English_New_Zeland = 2027,
        [StringValue("en-GB")]
        English_United_Kingdom = 11,
        [StringValue("es-ES")]
        Spanish_Spain = 2,
        [StringValue("es-MX")]
        Spanish_Mexico = 1021,
        [StringValue("fr-BE")]
        French_Belgique = 15,
        [StringValue("fr-CA")]
        French_Canada = 3,
        [StringValue("fr-FR")]
        French_France = 6,
        [StringValue("en-IE")]
        Gailge = 7,
        [StringValue("hr-HR")]
        Hrvatski = 18,
        [StringValue("it-IT")]
        Italy = 10,
        [StringValue("hu-HU")]
        Magyar = 5,
        [StringValue("nl-BE")]
        Nederlands_Belgie = 16,
        [StringValue("nl-NL")]
        Nederlands_Nederlands = 8,
        [StringValue("nn-NO")]
        Norks = 13,
        [StringValue("pt-PT")]
        Portugues = 2023,
        [StringValue("ro")]
        Romana = 21,
        [StringValue("sk-SK")]
        Slovencina = 2025,
        [StringValue("sl-SI")]
        Slovenski = 12,
        [StringValue("fi-FI")]
        Suomi = 1022,
        [StringValue("sv-SE")]
        Svenska = 17

    }



    public enum EnumOptionsMainMenu
    {
        [StringValue("Market Selection")]
        MarketSelection = 1,
        [StringValue("Product Selection")]
        ProductSelection = 2,
        [StringValue("Cart Summary")]
        CartSummary = 3,
        [StringValue("Review Order")]
        ReviewOrder = 4,
        [StringValue("Order Complete")]
        OrderComplete = 5
    }




    #region xEvent

    public enum EnumEventDisplayStatus
    {

        [StringValue("Inactive")]
        Inactive = 0,
        [StringValue("Running")]
        Running = 1,
        [StringValue("Cancelled")]
        Cancelled = 2,
        [StringValue("Closed")]
        Closed = 3

        //[StringValue("Cancelled")]
        //Cancelled = 1,
        //[StringValue("Active")]
        //Active = 2,
        //[StringValue("Deleted")]
        //Published = 3,
        //[StringValue("Created")]
        //Inactive = 4,
        //[StringValue("Suspended")]
        //Suspended = 5,
        //[StringValue("Deleted")]
        //Deleted = 6,
        //[StringValue("Running")]
        //Running = 7,
        //[StringValue("Expired")]
        //Expired = 8,
    }


    public enum EnumEventCreationMethod
    {
        [StringValue("A new event")]
        NewEvent = 0,
        [StringValue("Using event template")]
        UsingEventTemplate = 1,
        [StringValue("Using existing event")]
        UsingExistingEvent = 2,
        [StringValue("Event without registration")]
        WithoutRegistration = 3
    }

    public enum EnumEventSendEmailMethod
    {
        [StringValue("Set scheduled emails to be manually sent instead")]
        ManuallySent = 0,
        [StringValue("Automatically adjust the send dates of scheduled emails")]
        AutomaticallyAdjustSendDates = 1,
        [StringValue("Make ALL emails inactive and automatically adjust the send dates of scheduled emails")]
        MakeAllEmailsInactive = 2
    }

    public enum EnumEventRegistrationOpenTo
    {
        [StringValue("Anyone (Public) ")]
        AnyOne = 0,
        [StringValue("Only those on an invitation list (Private)")]
        InvitationList = 1,
        [StringValue("Only thoise who register from a xEvent email invitation (Invite-Only)")]
        RegisterFromxEvent = 2
    }

    #endregion



    #region InfoTrax
    public enum EnumAddressTypeInfoTrax
    {
        [StringValue("Shipping")]
        Shipping = 1, // IT MUST BE THE ID OF MERCHANT IN DATABASE
        [StringValue("Billing")]
        Billing = 2 // IT MUST BE THE ID OF MERCHANT IN DATABASE
    }

    public enum EnumItx_OrderSource
    {
        [StringValue("Orders/Shopping Cart")]
        OrdersShoppingCart = /*200,*/51,
        [StringValue("Enrollments")]
        Enrollments = 201, //52,
        [StringValue("Party")]
        Party = 202, //53,
        [StringValue("POS")]
        POS = 203, //54,
        [StringValue("Replicated site")]
        ReplicatedSite = 204, //55,
        [StringValue("Autoship")]
        Autoship = 205 //56
    }


    #endregion

    public enum EnumCompany
    {
        [StringValue("xirectss")]
        xirectss = 1
    }

    public enum EnumPayoutsStatus
    {
        [StringValue("Available")]
        Complete = 1,
        [StringValue("Requested")]
        Requested = 2,
        [StringValue("Issued")]
        Issued = 3,
        [StringValue("Rejected")]
        Rejected = 4,
        [StringValue("Deleted")]
        Deleted = 5,
        [StringValue("Pending")]
        Pending = 6
    }

    public enum EnumPayoutsType
    {
        [StringValue("Commissions")]
        Commissions = 1,
        //[StringValue("Hyperwallet")]
        //Hyperwallet = 2,
        [StringValue("Check")]
        Check = 3,
        [StringValue("Debt")]
        Debt = 4,
        [StringValue("Monthly Commision")]
        MonthlyCommision = 5,
    }

    public enum EnumModuleSettings
    {
        [StringValue("xReplicate")]
        xReplicate = 1,
        [StringValue("Recognition")]
        Recognition = 2,
        [StringValue("Media File Library")]
        MediaFileLibrary = 3,
        [StringValue("xSurveys")]
        xSurveys = 4,
        [StringValue("Home Party")]
        HomeParty = 5,
        [StringValue("xEvents")]
        xEvents = 6
    }
    public enum EnumPWBTriggerBy
    {
        [StringValue("Per Order")]
        PerOrder = 0,
        [StringValue("Per Distributor")]
        PerDistributor = 1
    }

    public enum EnumContinent
    {
        [StringValue("Africa")]
        Africa = 1,
        [StringValue("America")]
        America = 2,
        [StringValue("Asia")]
        Asia = 3,
        [StringValue("Australia y Oceania")]
        AustraliaOceania = 4,
        [StringValue("Europe")]
        Europe = 5
    }

    public enum EnumTypeSku {
        [StringValue("Types")]
        Types = 1,
        [StringValue("Countries")]
        Countries = 2,
        [StringValue("Languages")]
        Languages = 3
    }
    public enum EnumSkuSetupStatus
    {
        [StringValue("Active")]
        Active = 1,
        [StringValue("Deleted")]
        Deleted = 2        
    }


    public enum EnumDistBuyNowXMarkets
    {
        [StringValue("Australia")]
        Australia = 20,
        [StringValue("New Zealand")]
        NewZealand = 21,
        [StringValue("United States")]
        UnitedStates = 22,
        [StringValue("Austria")]
        Austria = 23,
        [StringValue("Belgium")]
        Belgium = 24,
        [StringValue("Canada")]
        Canada = 25,
        [StringValue("Croatia")]
        Croatia = 26,
        [StringValue("Czech Republic")]
        CzechRepublic = 27,
        [StringValue("Denmark")]
        Denmark = 28,
        [StringValue("Finland")]
        Finland = 29,
        [StringValue("France")]
        France = 30,
        [StringValue("Germany")]
        Germany = 31,
        [StringValue("Hungary")]
        Hungary = 32,
        [StringValue("Ireland")]
        Ireland =33,
        [StringValue("Italy")]
        Italy = 34,
        [StringValue("Mexico")]
        Mexico = 35,
        [StringValue("Netherlands")]
        Netherlands = 36,
        [StringValue("Norway")]
        Norway = 37,
        [StringValue("Portugal")]
        Portugal = 38,
        [StringValue("Romania")]
        Romania = 39,
        [StringValue("Slovakia")]
        Slovakia = 40,
        [StringValue("Slovenia")]
        Slovenia = 41,
        [StringValue("Spain")]
        Spain = 42,
        [StringValue("Sweden")]
        Sweden = 43,
        [StringValue("United Kingdom")]
        UnitedKingdom = 44,
        [StringValue("Switzerland")]
        Switzerland = 45
    }
    public enum EnumReportReconciliationNFR
    {
        [StringValue("All")]
        All = 0,
        [StringValue("Not Found in InfoTrax")]
        NotFoundInfoTrax = 2,
        [StringValue("Processing in InfoTrax")]
        ProcessingInfoTrax = 3,
        [StringValue("Order Total is different")]
        OrderTotaldifferent = 4,
        [StringValue("Matching")]
        Matching = 5
    }

    public enum EnumReportReconciliation
    {
        [StringValue("Enrollment (Orders Only)")]
        Enrollment = 7,
        [StringValue("Shopping Cart")]
        ShoppingCart = 8,
        [StringValue("Autoship Orders")]
        AutoshipOrders = 11,
    }

    public enum EnumTimeZones
    {
        [StringValue("Mountain Zone")]
        MountainZone = 9
    }

    public enum EnumPreferredPlacement
    {
        Balance = 10,
        LeftLeg = 20,
        RightLeg = 30
    }


    public enum EnumFormatReport
    {
        WebSite = 1,
        PDF = 2
    }
    public enum EnumPhoenixReports 
    {
        [StringValue("Rank_Advancements")]
        Rank_Advancements = 1,
        [StringValue("Autoship_Report")]
        Autoship_Report = 2,
        [StringValue("First_Time_Sponsors")]
        First_Time_Sponsors = 3,
        [StringValue("Inactive_Report")]
        Inactive_Report = 4,
        [StringValue("Director_300_Legs")]
        Director_300_Legs = 5,
        [StringValue("Volume_Totals")]
        Volume_Totals = 6,
        [StringValue("Order_Tracking")]
        Order_Tracking = 7,
        [StringValue("New_Customer_Associate_Search")]
        New_Customer_Associate_Search = 9,
        [StringValue("CustomerAssociate_Search")]
        CustomerAssociate_Search = 8,
    }

    public enum Enum_VO_FrequencyNotifications
    {
        [StringValue("Every Message")]
        Every_Message = 1,
        [StringValue("1 hour")]
        hour_1 = 2,
        [StringValue("6 hour")]
        hour_6 = 3,
        [StringValue("12 hour")]
        hour_12 = 4,
        [StringValue("1 day")]
        day_1 = 5,
    }

    public enum Enum_VO_TypeNotification
    {
        [StringValue("Email")]
        Email = 1,
        [StringValue("Phone")]
        Phone = 2,
        [StringValue("Text Message")]
        TextMessage = 3,
    }

    public enum Enum_VO_OptionMessage_Notification
    {
        [StringValue("Other Provider")]
        [StringMessage("")]
        OtherProvider = 1,

        [StringValue("AT&T Wireless")]
        [StringMessage("@txt.att.net")]
        ATTWireless = 2,

        [StringValue("Cingular")]
        [StringMessage("@cingularME.com")]
        Cingular = 3,

        [StringValue("Nextel")]
        [StringMessage("@messaging.nextell.com")]
        Nextel = 4,

        [StringValue("Qwest Wireless")]
        [StringMessage("@qwestmp.com")]
        QwestWireless = 5,

        [StringValue("Roger Wireless")]
        [StringMessage("@pcs.rogers.com")]
        RogerWireless = 6,

        [StringValue("SprintPCS")]
        [StringMessage("@messaging.sprintpcs.com")]
        SprintPCS = 7,

        [StringValue("T-Mobile")]
        [StringMessage("@tmomail.net")]
        TMobile = 8,

        [StringValue("Verizon")]
        [StringMessage("@vtext.com")]
        Verizon = 9,
    }
    public enum Enum_VO_MenuMessages
    {
        [StringValue("Send Message")]
        send_Message = 1,
        [StringValue("Messages")]
        messages = 2,
        [StringValue("Broadcast History")]
        broadcast_history = 3,
        [StringValue("Library")]
        library = 4,
        [StringValue("Events Calendar")]
        events_Calendar = 5,
        [StringValue("Contact Information Report")]
        contact_Information = 6,
        [StringValue("Notifications")]
        notifications = 7,
        [StringValue("Block Preferences")]
        block_Preferences = 8,
    }

    public enum EnumTipoEmpresa
    {
        [StringValue("Mediana")]
        Mediana = 1,
        [StringValue("Pequeña")]
        Pequeña = 2,
    }
}
