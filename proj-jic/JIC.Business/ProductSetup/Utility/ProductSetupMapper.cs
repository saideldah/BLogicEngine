using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JIC.DataAccess.ProductSetup.Entity;
using JIC.Business.ProductSetup.Model;

namespace JIC.Business.ProductSetup.Utility
{
    public class ProductSetupMapper
    {
        #region Model To Entity
        /// <summary>
        /// Map PropertyModel To PropertyEntity
        /// </summary>
        /// <param name="propertyModelObject"></param>
        /// <returns></returns>
        public static PropertyEntity PropertyModelToPropertyEntity(PropertyModel propertyModelObject)
        {
            PropertyEntity propertyEntityObject = new PropertyEntity();
            propertyEntityObject.Code = propertyModelObject.Code;
            propertyEntityObject.Title = propertyModelObject.Title;
            propertyEntityObject.PropertyType = propertyModelObject.PropertyType;
            propertyEntityObject.DataType = propertyModelObject.DataType;
            propertyEntityObject.Value = propertyModelObject.Value;
            return propertyEntityObject;
        }

        /// <summary>
        /// Map PropertyModel List To PropertyEntity List
        /// </summary>
        /// <param name="propertyModelObjectList"></param>
        /// <returns></returns>
        public static List<PropertyEntity> PropertyModelListToPropertyEntityList(List<PropertyModel> propertyModelObjectList)
        {
            List<PropertyEntity> propertyEntityObjectList = new List<PropertyEntity>();
            foreach (var propertyModelObject in propertyModelObjectList)
            {
                propertyEntityObjectList.Add(PropertyModelToPropertyEntity(propertyModelObject));
            }
            return propertyEntityObjectList;
        }

        /// <summary>
        /// Map Line Of Business Group Model To Line Of Business Group Entity
        /// </summary>
        /// <param name="lineOfBusinessGroupModelObject"></param>
        /// <returns></returns>
        public static LineOfBusinessGroupEntity LineOfBusinessGroupModelToLineOfBusinessGroupEntity(LineOfBusinessGroupModel lineOfBusinessGroupModelObject)
        {
            LineOfBusinessGroupEntity lineOfBusinessGroupEntityObject = new LineOfBusinessGroupEntity();
            lineOfBusinessGroupEntityObject.Code = lineOfBusinessGroupModelObject.Code;
            lineOfBusinessGroupEntityObject.Title = lineOfBusinessGroupModelObject.Title;
            return lineOfBusinessGroupEntityObject;
        }

        /// <summary>
        /// Map Line Of Business Group Model List To Line Of Business Group Entity List
        /// </summary>
        /// <param name="lineOfBusinessGroupModelObjectList"></param>
        /// <returns></returns>
        public static List<LineOfBusinessGroupEntity> LineOfBusinessGroupModelListToLineOfBusinessGroupEntityList(List<LineOfBusinessGroupModel> lineOfBusinessGroupModelObjectList)
        {
            List<LineOfBusinessGroupEntity> lineOfBusinessGroupEntityObjectList = new List<LineOfBusinessGroupEntity>();
            foreach (var lineOfBusinessGroupModelObject in lineOfBusinessGroupModelObjectList)
            {
                lineOfBusinessGroupEntityObjectList.Add(LineOfBusinessGroupModelToLineOfBusinessGroupEntity(lineOfBusinessGroupModelObject));
            }
            return lineOfBusinessGroupEntityObjectList;
        }


        /// <summary>
        /// Map Line Of Business Model To Line Of Business Entity
        /// </summary>
        /// <param name="lineOfBusinessModelObject"></param>
        /// <returns></returns>
        public static LineOfBusinessEntity LineOfBusinessModelToLineOfBusinessEntity(LineOfBusinessModel lineOfBusinessModelObject, Guid relatedLineOfBusinessGroupId)
        {
            LineOfBusinessEntity lineOfBusinessEntityObject = new LineOfBusinessEntity();
            lineOfBusinessEntityObject.Code = lineOfBusinessModelObject.Code;
            lineOfBusinessEntityObject.Title = lineOfBusinessModelObject.Title;
            lineOfBusinessEntityObject.GroupId = relatedLineOfBusinessGroupId;
            return lineOfBusinessEntityObject;
        }

        /// <summary>
        /// Map Line Of Business Model List To Line Of Business Entity List
        /// </summary>
        /// <param name="lineOfBusinessModelObjectList"></param>
        /// <returns></returns>
        public static List<LineOfBusinessEntity> LineOfBusinessModelListToLineOfBusinessEntityList(List<LineOfBusinessModel> lineOfBusinessModelObjectList, Guid relatedLineOfBusinessGroupId)
        {
            List<LineOfBusinessEntity> lineOfBusinessEntityObjectList = new List<LineOfBusinessEntity>();
            foreach (var lineOfBusinessModelObject in lineOfBusinessModelObjectList)
            {
                lineOfBusinessEntityObjectList.Add(LineOfBusinessModelToLineOfBusinessEntity(lineOfBusinessModelObject, relatedLineOfBusinessGroupId));
            }
            return lineOfBusinessEntityObjectList;
        }


        /// <summary>
        /// Map Package Model To Package Entity
        /// </summary>
        /// <param name="PackageModelObject"></param>
        /// <returns></returns>
        public static PackageEntity PackageModelToPackageEntity(PackageModel PackageModelObject, Guid relatedLineOfBusinessId)
        {
            PackageEntity PackageEntityObject = new PackageEntity();
            PackageEntityObject.Code = PackageModelObject.Code;
            PackageEntityObject.Title = PackageModelObject.Title;

            PackageEntityObject.SmiWizardType = PackageModelObject.SmiWizardType;
            PackageEntityObject.SavingsCalculator = PackageModelObject.SavingsCalculator;
            PackageEntityObject.Status = PackageModelObject.Status;

            PackageEntityObject.LineOfBusinessId = relatedLineOfBusinessId;
            return PackageEntityObject;
        }

        /// <summary>
        /// Map Package Model List To Package Entity List
        /// </summary>
        /// <param name="PackageModelObjectList"></param>
        /// <returns></returns>
        public static List<PackageEntity> PackageModelListToPackageEntityList(List<PackageModel> PackageModelObjectList, Guid relatedLineOfBusinessId)
        {
            List<PackageEntity> PackageEntityObjectList = new List<PackageEntity>();
            foreach (var PackageModelObject in PackageModelObjectList)
            {
                PackageEntityObjectList.Add(PackageModelToPackageEntity(PackageModelObject, relatedLineOfBusinessId));
            }
            return PackageEntityObjectList;
        }

        /// <summary>
        /// Map Product Model To Product Entity
        /// </summary>
        /// <param name="ProductModelObject"></param>
        /// <returns></returns>
        public static ProductEntity ProductModelToProductEntity(ProductModel ProductModelObject, Guid relatedPackageId)
        {
            ProductEntity ProductEntityObject = new ProductEntity();
            ProductEntityObject.Code = ProductModelObject.Code;
            ProductEntityObject.Title = ProductModelObject.Title;
            ProductEntityObject.PackageId = relatedPackageId;
            return ProductEntityObject;
        }

        /// <summary>
        /// Map Product Model List To Product Entity List
        /// </summary>
        /// <param name="productModelObjectList"></param>
        /// <returns></returns>
        public static List<ProductEntity> ProductModelListToProductEntityList(List<ProductModel> productModelObjectList, Guid relatedPackageId)
        {
            List<ProductEntity> productEntityObjectList = new List<ProductEntity>();
            foreach (var productModelObject in productModelObjectList)
            {
                productEntityObjectList.Add(ProductModelToProductEntity(productModelObject, relatedPackageId));
            }
            return productEntityObjectList;
        }


        /// <summary>
        /// Map Cover Model To Cover Entity
        /// </summary>
        /// <param name="CoverModelObject"></param>
        /// <returns></returns>
        public static CoverEntity CoverModelToCoverEntity(CoverModel CoverModelObject)
        {
            CoverEntity CoverEntityObject = new CoverEntity();
            CoverEntityObject.Code = CoverModelObject.Code;
            CoverEntityObject.Title = CoverModelObject.Title;
            CoverEntityObject.CodeBack = CoverModelObject.CodeBack;

            return CoverEntityObject;
        }

        /// <summary>
        /// Map Cover Model List To Cover Entity List
        /// </summary>
        /// <param name="CoverModelObjectList"></param>
        /// <returns></returns>
        public static List<CoverEntity> CoverModelListToCoverEntityList(List<CoverModel> CoverModelObjectList)
        {
            List<CoverEntity> CoverEntityObjectList = new List<CoverEntity>();
            foreach (var CoverModelObject in CoverModelObjectList)
            {
                CoverEntityObjectList.Add(CoverModelToCoverEntity(CoverModelObject));
            }
            return CoverEntityObjectList;
        }


        /// <summary>
        /// Map ProductMapping Model To ProductMapping Entity
        /// </summary>
        /// <param name="ProductMappingModelObject"></param>
        /// <returns></returns>
        public static ProductMappingEntity ProductMappingModelToProductMappingEntity(ProductMappingModel ProductMappingModelObject, Guid relatedProductId)
        {
            ProductMappingEntity ProductMappingEntityObject = new ProductMappingEntity();
            ProductMappingEntityObject.MappingRule = ProductMappingModelObject.MappingRule;
            ProductMappingEntityObject.CodeBack = ProductMappingModelObject.CodeBack;
            ProductMappingEntityObject.ProductId = relatedProductId;
            ProductMappingEntityObject.Status = ProductMappingModelObject.Status;



            return ProductMappingEntityObject;
        }

        /// <summary>
        /// Map ProductMapping Model List To ProductMapping Entity List
        /// </summary>
        /// <param name="ProductMappingModelObjectList"></param>
        /// <returns></returns>
        public static List<ProductMappingEntity> ProductMappingModelListToProductMappingEntityList(List<ProductMappingModel> ProductMappingModelObjectList, Guid relatedProductId)
        {
            List<ProductMappingEntity> ProductMappingEntityObjectList = new List<ProductMappingEntity>();
            foreach (var ProductMappingModelObject in ProductMappingModelObjectList)
            {
                ProductMappingEntityObjectList.Add(ProductMappingModelToProductMappingEntity(ProductMappingModelObject, relatedProductId));
            }
            return ProductMappingEntityObjectList;
        }


        public static SmiEntity SmiModelToSmiEntity(SmiModel SmiModelObject)
        {
            SmiEntity SmiEntityObject = new SmiEntity();
            SmiEntityObject.Code = SmiModelObject.Code;
            SmiEntityObject.Title = SmiModelObject.Title;
            SmiEntityObject.RetrievedFromBank = SmiModelObject.RetrievedFromBank;
            SmiEntityObject.FieldControlOnScreen = SmiModelObject.FieldControlOnScreen;
            SmiEntityObject.Description = SmiModelObject.Description;
            return SmiEntityObject;
        }

        /// <summary>
        /// Map SmiModel List To SmiEntity List
        /// </summary>
        /// <param name="SmiModelObjectList"></param>
        /// <returns></returns>
        public static List<SmiEntity> SmiModelListToSmiEntityList(List<SmiModel> SmiModelObjectList)
        {
            List<SmiEntity> SmiEntityObjectList = new List<SmiEntity>();
            foreach (var SmiModelObject in SmiModelObjectList)
            {
                SmiEntityObjectList.Add(SmiModelToSmiEntity(SmiModelObject));
            }
            return SmiEntityObjectList;
        }

        public static ProductSetupFileEntity ProductSetupFileModelToProductSetupFileEntity(ProductSetupFileModel productSetupFileModel)
        {
            ProductSetupFileEntity productSetupFileEntity = new ProductSetupFileEntity();
            productSetupFileEntity.LibraryName = productSetupFileModel.LibraryName;
            productSetupFileEntity.FolderPath = productSetupFileModel.FolderPath;
            productSetupFileEntity.OriginFileName = productSetupFileModel.OriginFileName;
            productSetupFileEntity.UploadedFileName = productSetupFileModel.UploadedFileName;
            productSetupFileEntity.FilePath = productSetupFileModel.FilePath;
            productSetupFileEntity.FileType = productSetupFileModel.FileType;
            productSetupFileEntity.Version = productSetupFileModel.Version;
            return productSetupFileEntity;
        }

        #endregion

        #region Entity To Model
        /// <summary>
        /// Map PropertyEntity To PropertyModel
        /// </summary>
        /// <param name="propertyModelObject"></param>
        /// <returns></returns>
        public static PropertyModel PropertyEntityToPropertyModel(PropertyEntity propertyEntityObject)
        {
            PropertyModel propertyModelObject = new PropertyModel();
            propertyModelObject.Id = propertyEntityObject.Id.ToString();
            propertyModelObject.Code = propertyEntityObject.Code;
            propertyModelObject.Title = propertyEntityObject.Title;
            propertyModelObject.PropertyType = propertyEntityObject.PropertyType;
            propertyModelObject.DataType = propertyEntityObject.DataType;
            return propertyModelObject;
        }

        /// <summary>
        /// Map PropertyEntity List To PropertyModel List
        /// </summary>
        /// <param name="propertyModelObjectList"></param>
        /// <returns></returns>
        public static List<PropertyModel> PropertyEntityListtOPropertyModelList(List<PropertyEntity> propertyEntityObjectList)
        {
            List<PropertyModel> propertyModelObjectList = new List<PropertyModel>();
            foreach (var propertyEntityObject in propertyEntityObjectList)
            {
                propertyModelObjectList.Add(PropertyEntityToPropertyModel(propertyEntityObject));
            }
            return propertyModelObjectList;
        }

        /// <summary>
        /// Map Line Of Business Group Entity To Line Of Business Group Model
        /// </summary>
        /// <param name="lineOfBusinessGroupModelObject"></param>
        /// <returns></returns>
        public static LineOfBusinessGroupModel LineOfBusinessGroupEntityToLineOfBusinessGroupModel(LineOfBusinessGroupEntity lineOfBusinessGroupEntityObject)
        {
            LineOfBusinessGroupModel lineOfBusinessGroupModelObject = new LineOfBusinessGroupModel();
            lineOfBusinessGroupModelObject.Id = lineOfBusinessGroupEntityObject.ToString();
            lineOfBusinessGroupModelObject.Code = lineOfBusinessGroupEntityObject.Code;
            lineOfBusinessGroupModelObject.Title = lineOfBusinessGroupEntityObject.Title;
            return lineOfBusinessGroupModelObject;
        }

        /// <summary>
        /// Map Line Of Business Group Entity List To Line Of Business Group Model List
        /// </summary>
        /// <param name="lineOfBusinessGroupModelObjectList"></param>
        /// <returns></returns>
        public static List<LineOfBusinessGroupModel> LineOfBusinessGroupEntityListToLineOfBusinessGroupModelList(List<LineOfBusinessGroupEntity> lineOfBusinessGroupEntityObjectList)
        {
            List<LineOfBusinessGroupModel> lineOfBusinessGroupModelObjectList = new List<LineOfBusinessGroupModel>();
            foreach (var lineOfBusinessGroupEntityObject in lineOfBusinessGroupEntityObjectList)
            {
                lineOfBusinessGroupModelObjectList.Add(LineOfBusinessGroupEntityToLineOfBusinessGroupModel(lineOfBusinessGroupEntityObject));
            }
            return lineOfBusinessGroupModelObjectList;
        }



        /// <summary>
        /// Map Line Of Business  Entity To Line Of Business  Model
        /// </summary>
        /// <param name="lineOfBusinessModelObject"></param>
        /// <returns></returns>
        public static LineOfBusinessModel LineOfBusinessEntityToLineOfBusinessModel(LineOfBusinessEntity lineOfBusinessEntityObject)
        {
            LineOfBusinessModel lineOfBusinessModelObject = new LineOfBusinessModel();
            lineOfBusinessModelObject.Id = lineOfBusinessEntityObject.ToString();
            lineOfBusinessModelObject.Code = lineOfBusinessEntityObject.Code;
            lineOfBusinessModelObject.Title = lineOfBusinessEntityObject.Title;
            return lineOfBusinessModelObject;
        }

        /// <summary>
        /// Map Line Of Business  Entity List To Line Of Business  Model List
        /// </summary>
        /// <param name="lineOfBusinessModelObjectList"></param>
        /// <returns></returns>
        public static List<LineOfBusinessModel> LineOfBusinessEntityListToLineOfBusinessModelList(List<LineOfBusinessEntity> lineOfBusinessEntityObjectList)
        {
            List<LineOfBusinessModel> lineOfBusinessModelObjectList = new List<LineOfBusinessModel>();
            foreach (var lineOfBusinessEntityObject in lineOfBusinessEntityObjectList)
            {
                lineOfBusinessModelObjectList.Add(LineOfBusinessEntityToLineOfBusinessModel(lineOfBusinessEntityObject));
            }
            return lineOfBusinessModelObjectList;
        }


        /// <summary>
        /// Map Package  Entity To Package  Model
        /// </summary>
        /// <param name="PackageModelObject"></param>
        /// <returns></returns>
        public static PackageModel PackageEntityToPackageModel(PackageEntity PackageEntityObject)
        {
            PackageModel PackageModelObject = new PackageModel();
            PackageModelObject.Id = PackageEntityObject.ToString();
            PackageModelObject.Code = PackageEntityObject.Code;
            PackageModelObject.Title = PackageEntityObject.Title;
            return PackageModelObject;
        }

        /// <summary>
        /// Map Package  Entity List To Package  Model List
        /// </summary>
        /// <param name="PackageModelObjectList"></param>
        /// <returns></returns>
        public static List<PackageModel> PackageEntityListToPackageModelList(List<PackageEntity> PackageEntityObjectList)
        {
            List<PackageModel> PackageModelObjectList = new List<PackageModel>();
            foreach (var PackageEntityObject in PackageEntityObjectList)
            {
                PackageModelObjectList.Add(PackageEntityToPackageModel(PackageEntityObject));
            }
            return PackageModelObjectList;
        }

        /// <summary>
        /// Map Product  Entity To Product  Model
        /// </summary>
        /// <param name="ProductModelObject"></param>
        /// <returns></returns>
        public static ProductModel ProductEntityToProductModel(ProductEntity ProductEntityObject)
        {
            ProductModel ProductModelObject = new ProductModel();
            ProductModelObject.Id = ProductEntityObject.ToString();
            ProductModelObject.Code = ProductEntityObject.Code;
            ProductModelObject.Title = ProductEntityObject.Title;
            return ProductModelObject;
        }

        /// <summary>
        /// Map Product  Entity List To Product  Model List
        /// </summary>
        /// <param name="ProductModelObjectList"></param>
        /// <returns></returns>
        public static List<ProductModel> ProductEntityListToProductModelList(List<ProductEntity> ProductEntityObjectList)
        {
            List<ProductModel> ProductModelObjectList = new List<ProductModel>();
            foreach (var ProductEntityObject in ProductEntityObjectList)
            {
                ProductModelObjectList.Add(ProductEntityToProductModel(ProductEntityObject));
            }
            return ProductModelObjectList;
        }


        /// <summary>
        /// Map Cover  Entity To Cover  Model
        /// </summary>
        /// <param name="CoverModelObject"></param>
        /// <returns></returns>
        public static CoverModel CoverEntityToCoverModel(CoverEntity CoverEntityObject)
        {
            CoverModel CoverModelObject = new CoverModel();
            CoverModelObject.Id = CoverEntityObject.ToString();
            CoverModelObject.Code = CoverEntityObject.Code;
            CoverModelObject.Title = CoverEntityObject.Title;
            return CoverModelObject;
        }

        /// <summary>
        /// Map Cover  Entity List To Cover  Model List
        /// </summary>
        /// <param name="CoverModelObjectList"></param>
        /// <returns></returns>
        public static List<CoverModel> CoverEntityListToCoverModelList(List<CoverEntity> CoverEntityObjectList)
        {
            List<CoverModel> CoverModelObjectList = new List<CoverModel>();
            foreach (var CoverEntityObject in CoverEntityObjectList)
            {
                CoverModelObjectList.Add(CoverEntityToCoverModel(CoverEntityObject));
            }
            return CoverModelObjectList;
        }
        public static SmiModel SmiEntityToSmiModel(SmiEntity SmiEntityObject)
        {
            SmiModel SmiModelObject = new SmiModel();
            SmiModelObject.Code = SmiEntityObject.Code;
            SmiModelObject.Title = SmiEntityObject.Title;
            SmiModelObject.RetrievedFromBank = SmiEntityObject.RetrievedFromBank;
            SmiModelObject.FieldControlOnScreen = SmiEntityObject.FieldControlOnScreen;
            SmiModelObject.Description = SmiEntityObject.Description;
            return SmiModelObject;
        }

        /// <summary>
        /// Map SmiModel List To SmiEntity List
        /// </summary>
        /// <param name="SmiModelObjectList"></param>
        /// <returns></returns>
        public static List<SmiModel> SmiEntityListToSmiModelList(List<SmiEntity> SmiEntityObjectList)
        {
            List<SmiModel> SmiModelObjectList = new List<SmiModel>();
            foreach (var SmiEntityObject in SmiEntityObjectList)
            {
                SmiModelObjectList.Add(SmiEntityToSmiModel(SmiEntityObject));
            }
            return SmiModelObjectList;
        }


        public static ProductSetupFileModel ProductSetupFileEntityToProductSetupFileModel(ProductSetupFileEntity productSetupFileEntity )
        {
            ProductSetupFileModel productSetupFileModel = new ProductSetupFileModel();
            productSetupFileModel.Id = productSetupFileEntity.Id.ToString();
            productSetupFileModel.LibraryName = productSetupFileEntity.LibraryName;
            productSetupFileModel.FolderPath = productSetupFileEntity.FolderPath;
            productSetupFileModel.OriginFileName = productSetupFileEntity.OriginFileName;
            productSetupFileModel.UploadedFileName = productSetupFileEntity.UploadedFileName;
            productSetupFileModel.FilePath = productSetupFileEntity.FilePath;
            productSetupFileModel.FileType = productSetupFileEntity.FileType;
            productSetupFileModel.CreatedDate = productSetupFileEntity.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            productSetupFileModel.Version = productSetupFileEntity.Version;
            return productSetupFileModel;
        }
        public static List<ProductSetupFileModel> ProductSetupFileEntityListToProductSetupFileModelList(List<ProductSetupFileEntity> productSetupFileEntityList)
        {
            List<ProductSetupFileModel> productSetupFileModelList = new List<ProductSetupFileModel>();
            foreach (var productSetupFileEntity in productSetupFileEntityList)
            {
                productSetupFileModelList.Add(ProductSetupFileEntityToProductSetupFileModel(productSetupFileEntity));
            }
            return productSetupFileModelList;
        }

        #endregion
    }
}
