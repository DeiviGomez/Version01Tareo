using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Library.General
{
    public class clsSerialize
    {

    }

    //Example of nodes
    //{ 
    //                "data" : "A node", 
    //                "metadata" : { id : 23 },
    //                "children" : [ 
						
    //                    { 
    //                        "data" : "A node1", 
    //                        "metadata" : { id : 23 }
    //                    },						
    //                    { 
    //                        "data" : "A node2", 
    //                        "metadata" : { id : 24 }
    //                    } 
						
    //                    ]
    //            },
				
    //            { 
    //                        "data" : "A nodegrande", 
    //                        "metadata" : { id : 24 }
    //            }

   public class srGenealogyItem   
   {
       public String data { get; set; }
       public srMetadata attr { get; set; }
       public List<srGenealogyItem> children { get; set; }
   }

   public class srGenealogyItemEdit
   {
       public String data { get; set; }
       public srMetadataEdit attr { get; set; }
       public List<srGenealogyItemEdit> children { get; set; }
   }

   public class srGenealogyItem_Edit
   {
       public String data { get; set; }
       public srMetadata_Edit attr { get; set; }
       public List<srGenealogyItem_Edit> children { get; set; }
   }

   public class srMetadata_Edit
   {
       public String id { get; set; }
       public String enrollerid { get; set; }
       public String placementid { get; set; }
       public String LegacyNumber { get; set; }
       public String Edit { get; set; }
   }

   public class srMetadataEdit
   {
       public String id { get; set; }
       public String enrollerid { get; set; }
       public String placementid { get; set; }
       public String LegacyNumber { get; set; }
       public String Edit { get; set; }
   }

   [Serializable]
   public class srLenguages 
   {
       public String id { get; set; }
       public String LenguageName { get; set; }
       public String FolderExtension { get; set; }
       public String Icon { get; set; }
       public String Status { get; set; }
       public String Published { get; set; }
       public String CultureInfo { get; set; }
       public String LegacyLanguage { get; set; }
       
   }

   public class srMetadata
   {
       public String id { get; set; }
       public String enrollerid { get; set; }
       public String placementid { get; set; }
       public String LegacyNumber { get; set; }
   }

   [Serializable]
   public class srDistributorDownlineReview
   {
       public String DistributorID { get; set; }
       public String LegacyNumber { get; set; }
       public String DistributorName { get; set; }
       public String RankName { get; set; }
       public String JoinedDate { get; set; }
       public String TasksCompleted { get; set; }
       public String TasksPastDue { get; set; }
       public String Overrall { get; set; }
   }

   [Serializable]
   public class srCatalogTree
   {
       public String Id { get; set; }
       public String Name { get; set; }
       public String Parent { get; set; }
       public String Status { get; set; }
   }




   [Serializable]
   public class srEventDetail
   {
       public String host { get; set; }
       public String title { get; set; }
       public String type { get; set; }
       public String cost { get; set; }
       public String contact { get; set; }
       public String contact_phone { get; set; }
       public String speakers { get; set; }
       public String location { get; set; }
       public String language { get; set; }
       public String Date { get; set; }
       public String latitude { get; set; }
       public String longitude { get; set; }
       public String existsposition { get; set; }
       public String notes { get; set; }
       public String varLocation { get; set; }
       public String varLongitude { get; set; }
   }

}
