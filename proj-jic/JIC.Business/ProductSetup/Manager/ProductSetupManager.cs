using System.IO;
using JIC.DataAccess.ProductSetup;
using JIC.Business.Model;
using System.Collections.Generic;
using JIC.Business.ProductSetup.Model;
using LibraryApp.Client;
using BSynchro.ExcelReader;
using System;
using System.Linq;
using JIC.DataAccess.ProductSetup.Entity;
using JIC.Business.ProductSetup.Utility;
using JIC.DataAccess.ProductSetup.Repository;

namespace JIC.Business.ProductSetup.Manager
{
    internal class ProductExcelNode
    {
        public string Code { set; get; }
        public string Title { set; get; }

        public List<ProductExcelNode> ParentProducts = new List<ProductExcelNode>();

        public List<ProductExcelNode> ChiledProducts = new List<ProductExcelNode>();
        public int ChildCounts { set; get; }
    }

    public class ProductSetupManager
    {


        #region Private Fields
        private const string LIBRARY_APP = "Library";

        #endregion

        #region Private Properties
        private PropertyRepository PropertyRepositoryObject { get; set; }
        private LineOfBusinessGroupRepository LineOfBusinessGroupRepositoryObject { get; set; }
        private LineOfBusinessRepository LineOfBusinessRepositoryObject { get; set; }
        private PackageRepository PackageRepositoryObject { get; set; }
        private ProductRepository ProductRepositoryObject { get; set; }
        private CoverRepository CoverRepositoryObject { get; set; }
        private CoverMappingRepository CoverMappingRepositoryObject { get; set; }
        private ProductSetupRepository ProductSetupRepositoryObject { get; set; }

        #endregion

        #region Constructors
        public ProductSetupManager()
        {
            PropertyRepositoryObject = PropertyRepository.CreateInstance();
            LineOfBusinessGroupRepositoryObject = LineOfBusinessGroupRepository.CreateInstance();
            LineOfBusinessRepositoryObject = LineOfBusinessRepository.CreateInstance();
            PackageRepositoryObject = PackageRepository.CreateInstance();
            ProductRepositoryObject = ProductRepository.CreateInstance();
            CoverRepositoryObject = CoverRepository.CreateInstance();
            ProductSetupRepositoryObject = ProductSetupRepository.CreateInstance();
            CoverMappingRepositoryObject = CoverMappingRepository.CreateInstance();
        }
        #endregion

        #region Public Properties

        #endregion

        #region Private Methods


        /// <summary>
        /// Validate product Setup File Model
        /// </summary>
        /// <param name="productSetupFileModel"> productSetupFileModel payload</param>
        /// <returns></returns>
        private List<ErrorModel> ValidateProductSetupFileModel(ProductSetupFileModel productSetupFileModel)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();
            if (productSetupFileModel == null)
            {
                errorList.Add(new ErrorModel()
                {
                    Type = ErrorType.ValidationError,
                    Message = "ProductSetupFileModel should not be null"
                });
                return errorList;
            }
            else
            {
                if (string.IsNullOrEmpty(productSetupFileModel.FilePath))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "FilePath should not be null or empty"
                    });
                }
                if (string.IsNullOrEmpty(productSetupFileModel.LibraryName))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "LibraryName should not be null or empty"
                    });
                }
                if (string.IsNullOrEmpty(productSetupFileModel.FolderPath))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "FolderPath should not be null or empty"
                    });
                }
                if (productSetupFileModel.Version > 0)
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "version should not be null or less than 0"
                    });
                }
                if (string.IsNullOrEmpty(productSetupFileModel.UploadedFileName))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "UploadedFileName should not be null or empty"
                    });
                }
                if (string.IsNullOrEmpty(productSetupFileModel.OriginFileName))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "OriginFileName should not be null or empty"
                    });
                }
                if (string.IsNullOrEmpty(productSetupFileModel.FileType))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "FileType should not be null or empty"
                    });
                }
            }
            return errorList;
        }
        /// <summary>
        /// Map workbook to ProductSetupModel Object
        /// </summary>
        /// <param name="uploadedExcelWorkbook"></param>
        /// <returns></returns>
        private ProductSetupModel WorkbookToProductSetupModel(Workbook uploadedExcelWorkbook)
        {
            ProductSetupModel roductSetupModel = new ProductSetupModel();
            return roductSetupModel;
        }
        /// <summary>
        /// Retrieve File from library app and assign it to WorkBook 
        /// </summary>
        /// <param name="filePath">FilePath in Library App</param>
        /// <returns></returns>
        private Workbook RetrieveFileWorkbook(string filePath)
        {
            var libraryClient = new LibraryClient(LIBRARY_APP);
            var file = libraryClient.GetFileSync(filePath);
            byte[] fileContent = file.Content;
            Workbook workbook = new Workbook(fileContent);
            return workbook;
        }

        /// <summary>
        /// Validate Product Setup Workbook Schema before mapping, this function validates only the schema of worksheets by comparing it with template 
        /// </summary>
        /// <param name="productSetuWorkbook"></param>
        /// <returns></returns>
        private List<ErrorModel> ValidateProductSetupWorkBookSchema(Workbook productSetuWorkbook)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();
            //preprocessors code to specify if we are in debug or release mode
            string templateFilePaths = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "bin", "Controllers", "ProductSetup", "Template", "ProductSetupTemplate.xlsx");


            if (!File.Exists(templateFilePaths))
            {
                errorList.Add(
                    new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "Template File Not Found"
                    }
                    );
                return errorList;
            }


            byte[] templateFileContent = File.ReadAllBytes(templateFilePaths);
            Workbook templateFileWorkbook = new Workbook(templateFileContent);


            IList<Worksheet> productSetupWorkSheetList = productSetuWorkbook.Worksheets;
            IList<Worksheet> productSetupTemplateWorkSheetList = templateFileWorkbook.Worksheets;

            if (productSetupTemplateWorkSheetList.Count != productSetupWorkSheetList.Count)
            {
                errorList.Add(new ErrorModel()
                {
                    Type = ErrorType.ValidationError,
                    Message = "The uploaded excel file is not valid. The uploaded file should contain '" + productSetupTemplateWorkSheetList.Count + "' sheets"
                });
            }
            foreach (var templateWorksheet in productSetupTemplateWorkSheetList)
            {
                if (!productSetupWorkSheetList.Any(w => w.Name == templateWorksheet.Name))
                {
                    errorList.Add(new ErrorModel()
                    {
                        Type = ErrorType.ValidationError,
                        Message = "The uploaded excel file is not valid. The sheet '" + templateWorksheet.Name + "' is not found"
                    });
                }
                else
                {
                    // Validate Sheet Schema
                    Worksheet productSetupWorkSheet = productSetupWorkSheetList.FirstOrDefault(w => w.Name == templateWorksheet.Name);
                    Worksheet productSetupTemplateWorkSheet = productSetupTemplateWorkSheetList.FirstOrDefault(w => w.Name == templateWorksheet.Name);

                    if (productSetupWorkSheet != null)
                    {
                        List<Cell> headers = productSetupWorkSheet.Rows[0].Cells.Where(c => c != null).ToList();
                        List<Cell> templateHeaders = productSetupTemplateWorkSheet.Rows[0].Cells.Where(c => c != null).ToList();
                        foreach (var cell in templateHeaders)
                        {
                            if (cell != null)
                            {
                                if (!headers.Any(c => c.Value == cell.Value))
                                {
                                    errorList.Add(new ErrorModel()
                                    {
                                        Type = ErrorType.ValidationError,
                                        Message = "The uploaded excel file is not valid. The header '" + cell.Value + "' in '" + templateWorksheet.Name + "' sheet is not found"
                                    });
                                }
                            }
                        }
                    }//end if (productSetupWorkSheet != null)
                }//end else
            }//end for



            return errorList;
        }


        /// <summary>
        /// Function to Map Product Setup Workbook To Product Setup Model 
        /// </summary>
        /// <param name="productSetupWorkbook">should be valid Product Setup Workbook</param>
        /// <returns></returns>
        private ProductSetupModel MapProductSetupWorkbookToProductSetupModel(Workbook productSetupWorkbook)
        {
            IList<Worksheet> productSetupWorksheetList = productSetupWorkbook.Worksheets;
            ProductSetupModel productSetupModel = new ProductSetupModel();


            #region Map Product Selections Sheet to productSetupModel.LineOfBusinessGroupList
            Worksheet productSelectionsSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Product Selections".ToInternalName());

            //Map To recursive object

            List<ProductExcelNode> rootProductExcelNodeList = new List<ProductExcelNode>();
            List<ProductExcelNode> pointer = rootProductExcelNodeList;
            List<ProductExcelNode> parentList = rootProductExcelNodeList;

            ProductExcelNode productExcelNode = new ProductExcelNode();

            for (int row = 1; row < productSelectionsSheet.Rows.Count; row++)
            {
                for (int col = 0; col < productSelectionsSheet.Rows[0].Cells.Count; col += 2)
                {
                    if (productSelectionsSheet.Rows[row].Cells[col] != null)
                    {
                        if (!string.IsNullOrEmpty(productSelectionsSheet.Rows[row].Cells[col].Value))
                        {
                            productExcelNode = new ProductExcelNode();
                            productExcelNode.Code = productSelectionsSheet.Rows[row].Cells[col].Value;
                            productExcelNode.Title = productSelectionsSheet.Rows[row].Cells[col + 1].Value;
                            productExcelNode.ParentProducts = parentList;

                            #region Map single child multiple parents
                            if (productSelectionsSheet.Rows.Count > row + 1)
                            {
                                if (productSelectionsSheet.Rows[row + 1].Cells[col] == null)
                                {
                                    if (parentList.Count > 1)
                                    {
                                        foreach (var item in productExcelNode.ParentProducts)
                                        {
                                            item.ChiledProducts.Add(productExcelNode);
                                        }
                                    }
                                }
                                else
                                {
                                    if (parentList.Count > 1 && string.IsNullOrEmpty(productSelectionsSheet.Rows[row + 1].Cells[col].Value))
                                    {
                                        foreach (var item in productExcelNode.ParentProducts)
                                        {
                                            item.ChiledProducts.Add(productExcelNode);
                                        }
                                    }
                                }
                            }

                            #endregion


                            pointer.Add(productExcelNode);

                        }
                        else
                        {
                            #region Map single child multiple parents
                            if (parentList.Count > 1)
                            {
                                foreach (var item in parentList)
                                {
                                    item.ChiledProducts = parentList.FirstOrDefault().ChiledProducts;
                                }
                            }

                            #endregion
                        }
                        if (pointer.Count > 0)
                        {
                            parentList = pointer;
                            pointer = pointer.Last().ChiledProducts;
                        }
                    }
                }
                pointer = rootProductExcelNodeList;
            }

            //Map LineOfBusinessGroupList
            foreach (var lineOfBusinessGroupExcelModel in rootProductExcelNodeList)
            {

                var lineOfBusinessGroupModel = new LineOfBusinessGroupModel()
                {
                    Code = lineOfBusinessGroupExcelModel.Code,
                    Title = lineOfBusinessGroupExcelModel.Title
                };

                //Map LineOfBusinessList
                var lineOfBusinessExcelModelList = lineOfBusinessGroupExcelModel.ChiledProducts;
                foreach (var lineOfBusinessExcelModelItem in lineOfBusinessExcelModelList)
                {
                    var lineOfBusinessModel = new LineOfBusinessModel()
                    {
                        Code = lineOfBusinessExcelModelItem.Code,
                        Title = lineOfBusinessExcelModelItem.Title
                    };

                    var packagesExcelModelList = lineOfBusinessExcelModelItem.ChiledProducts;
                    //Map Packages
                    foreach (var packagesExcelModelItem in packagesExcelModelList)
                    {
                        var packageModel = new PackageModel()
                        {
                            Code = packagesExcelModelItem.Code,
                            Title = packagesExcelModelItem.Title
                        };
                        var productExcelModelList = packagesExcelModelItem.ChiledProducts;
                        //Map Products
                        foreach (var productExcelModelItem in productExcelModelList)
                        {
                            var productModel = new ProductModel()
                            {
                                Code = productExcelModelItem.Code,
                                Title = productExcelModelItem.Title
                            };
                            packageModel.ProductList.Add(productModel);
                        }
                        lineOfBusinessModel.PackageList.Add(packageModel);
                    }

                    lineOfBusinessGroupModel.LineOfBusinessList.Add(lineOfBusinessModel);
                }

                productSetupModel.LineOfBusinessGroupList.Add(lineOfBusinessGroupModel);
            }



            #endregion


            #region Map packageParametersSheet to lineOfBusiness Property List and Product Setup Property List
            Worksheet packageParametersSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Package Parameters".ToInternalName());

            #region Extract Property
            for (int row = 1; row < packageParametersSheet.Rows.Count; row++)
            {
                for (int col = 1; col < packageParametersSheet.Rows[row].Cells.Count; col++)
                {
                    if (packageParametersSheet.Rows[row].Cells[0] != null)
                    {
                        if (!string.IsNullOrEmpty(packageParametersSheet.Rows[row].Cells[0].Value))
                        {
                            var lineOfBusinessCode = packageParametersSheet.Rows[row].Cells[0].Value;
                            var lineOfBusinessProperty = packageParametersSheet.Rows[0].Cells[col].Value;
                            var lineOfBusinessPropertyValue = packageParametersSheet.Rows[row].Cells[col].Value;
                            #region assign property to is ProductSetupModel Object

                            ////LineOfBusinessGroup
                            foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
                            {
                                //LineOfBusiness
                                var lineOfBusiness = lineOfBusinessGroup.LineOfBusinessList.FirstOrDefault(lob => lob.Code == lineOfBusinessCode);
                                if (lineOfBusiness != null)
                                {
                                    var property = new PropertyModel();
                                    property.Code = lineOfBusinessProperty.ToInternalName();
                                    property.Title = lineOfBusinessProperty;
                                    if (string.IsNullOrEmpty(lineOfBusinessPropertyValue))
                                    {
                                        property.Value = "no";
                                    }
                                    else
                                    {
                                        property.Value = "yes";
                                    }
                                    //add the property to line of lineOfBusiness object
                                    lineOfBusiness.PropertyList.Add(property);

                                    //add the property to propertyList in product setup object
                                    var productSetupObjectProperty = new PropertyModel(property);
                                    productSetupObjectProperty.Value = "";
                                    if (!productSetupModel.PropertyList.Any(p => p.Code == productSetupObjectProperty.Code))
                                    {
                                        productSetupModel.PropertyList.Add(productSetupObjectProperty);
                                    }

                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region Map packageParameterDefinitionSheet
            Worksheet packageParameterDefinitionSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Package Parameter Definition".ToInternalName());
            #region Extract Property

            var parameterNameList = GetColumn("Parameter Name", packageParameterDefinitionSheet);
            var dataTypeList = GetColumn("Data Type", packageParameterDefinitionSheet);
            var parameterTypeList = GetColumn("Parameter Type", packageParameterDefinitionSheet);
            if (parameterNameList.Count != dataTypeList.Count || parameterNameList.Count != parameterTypeList.Count)
            {
                throw new Exception("Parameter Name, Data Type, Parameter Type in Package Parameter Definition should have same values count");
            }


            for (int i = 0; i < parameterNameList.Count; i++)
            {
                var propertyCode = parameterNameList[i].ToInternalName();
                var propertyTitle = parameterNameList[i];
                var propertyDataType = dataTypeList[i];
                var parameterType = parameterTypeList[i];
                #region assign property to is ProductSetupModel Object


                //update property to propertyList in product setup object
                var property = productSetupModel.PropertyList.FirstOrDefault(p => p.Code == propertyCode);
                if (property == null)
                {
                    property = new PropertyModel()
                    {
                        Code = propertyCode,
                        Title = propertyTitle,
                        DataType = propertyDataType,
                        PropertyType = parameterType
                    };
                    productSetupModel.PropertyList.Add(property);
                }
                else
                {
                    property.DataType = propertyDataType;
                    property.PropertyType = parameterType;
                }


                //LineOfBusinessGroup
                foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
                {
                    //LineOfBusiness
                    foreach (var lineOfBusiness in lineOfBusinessGroup.LineOfBusinessList)
                    {
                        var lineOfBusinessProperty = lineOfBusiness.PropertyList.FirstOrDefault(p => p.Code == propertyCode);
                        if (lineOfBusinessProperty != null)
                        {
                            lineOfBusinessProperty.DataType = propertyDataType;
                            lineOfBusinessProperty.PropertyType = parameterType;
                        }

                    }
                }
                #endregion
            }

            #endregion
            #endregion

            #region Map smiSheet
            Worksheet smiSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "SMI".ToInternalName());
            //retrieve sheet columns
            var smiPropertiesList = GetColumn("Properties", smiSheet);
            var smiApplicablePackagePropertiesList = GetColumn("Applicable Package Properties", smiSheet);
            var smiTypeList = GetColumn("Type", smiSheet);
            var smiRetrievedFromBankList = GetColumn("Retrieved from bank", smiSheet);
            var smiFieldControlOnScreenList = GetColumn("Field Control On screen", smiSheet);
            var smiDescriptionList = GetColumn("Description", smiSheet);


            if (
                smiPropertiesList.Count != smiApplicablePackagePropertiesList.Count ||
                smiPropertiesList.Count != smiTypeList.Count ||
                smiPropertiesList.Count != smiRetrievedFromBankList.Count ||
                smiPropertiesList.Count != smiFieldControlOnScreenList.Count ||
                smiPropertiesList.Count != smiDescriptionList.Count
                )
            {
                throw new Exception("Properties, Applicable Package Properties, Type, Retrieved from bank, Field Control On screen and Description fields in Package Parameter Definition should have same values count");
            }
            for (int i = 0; i < smiPropertiesList.Count; i++)
            {
                var smiModel = new SmiModel();
                smiModel.Code = smiPropertiesList[i].ToInternalName();
                smiModel.Title = smiPropertiesList[i];
                var smiProperties = smiApplicablePackagePropertiesList[i].Split(',');

                //add smi properties
                foreach (var smiProperty in smiProperties)
                {
                    var propertyModel = new PropertyModel();
                    propertyModel.Code = smiProperty.ToInternalName();
                    propertyModel.Title = smiProperty;
                    propertyModel.DataType = smiTypeList[i];
                    smiModel.PropertyList.Add(propertyModel);

                    if (!productSetupModel.PropertyList.Any(p => p.Code == propertyModel.Code))
                    {
                        productSetupModel.PropertyList.Add(propertyModel);
                    }
                }

                if (string.IsNullOrEmpty(smiRetrievedFromBankList[i]))
                {
                    smiModel.RetrievedFromBank = false;
                }
                else
                {
                    smiModel.RetrievedFromBank = true;
                }

                smiModel.FieldControlOnScreen = smiFieldControlOnScreenList[i];
                smiModel.Description = smiDescriptionList[i];

                productSetupModel.SmiList.Add(smiModel);
            }
            #endregion


            #region Map packagesSheet
            Worksheet packagesSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Packages".ToInternalName());

            #region Extract Property
            for (int row = 1; row < packagesSheet.Rows.Count; row++)
            {
                for (int col = 1; col < packagesSheet.Rows[row].Cells.Count; col++)
                {
                    if (packagesSheet.Rows[row].Cells[0] != null)
                    {
                        if (!string.IsNullOrEmpty(packagesSheet.Rows[row].Cells[0].Value))
                        {
                            var packageCode = packagesSheet.Rows[row].Cells[0].Value;
                            var packageProperty = packagesSheet.Rows[0].Cells[col].Value;
                            var packagePropertyValue = packagesSheet.Rows[row].Cells[col].Value;



                            #region assign property to is ProductSetupModel Object
                            ////LineOfBusinessGroup
                            foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
                            {
                                //LineOfBusiness
                                foreach (var lineOfBusiness in lineOfBusinessGroup.LineOfBusinessList)
                                {
                                    var package = lineOfBusiness.PackageList.FirstOrDefault(pac => pac.Code == packageCode);
                                    if (package != null)
                                    {
                                        var property = new PropertyModel();
                                        property.Code = packageProperty.ToInternalName();
                                        property.Title = packageProperty;
                                        property.Value = packagePropertyValue;

                                        //add the property to line of lineOfBusiness object
                                        package.PropertyList.Add(property);

                                        //add the property to propertyList in product setup object
                                        var productSetupObjectProperty = new PropertyModel(property);
                                        productSetupObjectProperty.Value = "";
                                        if (!productSetupModel.PropertyList.Any(p => p.Code == productSetupObjectProperty.Code))
                                        {
                                            productSetupModel.PropertyList.Add(productSetupObjectProperty);
                                        }

                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion
            #endregion

            #region Map packageSettingsSheet
            Worksheet packageSettingsSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Package Settings".ToInternalName());
            var packageList = GetColumn("Package", packageSettingsSheet);
            var smiWizardTypeList = GetColumn("SmiWizardType", packageSettingsSheet);
            var savingsCalculatorList = GetColumn("SavingsCalculator", packageSettingsSheet);
            var statusList = GetColumn("Status", packageSettingsSheet);
            if (
            packageList.Count != smiWizardTypeList.Count ||
            packageList.Count != savingsCalculatorList.Count ||
            packageList.Count != statusList.Count
            )
            {
                throw new Exception("Package, SmiWizardType, SavingsCalculator and Status fields in Package Settings sheet should have the same values count");
            }

            for (int i = 0; i < packageList.Count; i++)
            {
                ////LineOfBusinessGroup
                foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
                {
                    //LineOfBusiness
                    foreach (var lineOfBusiness in lineOfBusinessGroup.LineOfBusinessList)
                    {
                        var package = lineOfBusiness.PackageList.FirstOrDefault(pac => pac.Code == packageList[i]);
                        if (package != null)
                        {
                            package.SmiWizardType = smiWizardTypeList[i];
                            package.SavingsCalculator = savingsCalculatorList[i];
                            package.Status = statusList[i];
                        }
                    }
                }
            }
            #endregion

            #region Map coverBenefitsSheet
            Worksheet coverBenefitsSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Cover Benefits".ToInternalName());
            var coverNameList = GetColumnNonEmptyValues("Name", coverBenefitsSheet);
            var coverCodeList = GetColumnNonEmptyValues("Code", coverBenefitsSheet);
            var coverPropertyList = GetGroupedColumnNonEmptyValues("Properties", coverBenefitsSheet);

            Worksheet otherSubTypeCoverMappingSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Other SubType Cover Mapping".ToInternalName());
            var foCoverCodeList = GetColumn("FO Cover Code", otherSubTypeCoverMappingSheet);
            var boCoverCodeList = GetColumn("BO Cover Code", otherSubTypeCoverMappingSheet);

            if (foCoverCodeList.Count != foCoverCodeList.Count)
            {
                throw new Exception("'FO Cover Code' and 'BO Cover Code' fields in 'Other SubType Cover Mapping' sheet should have the same values count");
            }
            #region for each LineOfBusinessGroup
            foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
            {
                #region for each LineOfBusiness
                foreach (var lineOfBusiness in lineOfBusinessGroup.LineOfBusinessList)
                {
                    #region for each  package
                    foreach (var package in lineOfBusiness.PackageList)
                    {
                        #region fill product cover property value
                        for (int col = 3; col < coverBenefitsSheet.Rows[0].Cells.Count; col++)
                        {
                            if (coverBenefitsSheet.Rows[0].Cells[col] != null)
                            {
                                var productCode = coverBenefitsSheet.Rows[0].Cells[col].Value;

                                var product = package.ProductList.FirstOrDefault(p => p.Code == productCode);
                                if (product != null)
                                {
                                    int coverPropertyOffset = 1;
                                    for (int coverNameAndCodeCounter = 0; coverNameAndCodeCounter < coverNameList.Count; coverNameAndCodeCounter++)
                                    {
                                        var cover = new CoverModel();
                                        cover.Code = coverCodeList[coverNameAndCodeCounter];
                                        cover.Title = coverNameList[coverNameAndCodeCounter];
                                        var rowCount = coverBenefitsSheet.Rows.Count(r => !r.IsEmpty);
                                        for (int coverPropertyCounter = 0; coverPropertyCounter < coverPropertyList.Count; coverPropertyCounter++)
                                        {
                                            var property = new PropertyModel();
                                            property.Code = coverPropertyList[coverPropertyCounter].ToInternalName(); ;
                                            property.Title = coverPropertyList[coverPropertyCounter];
                                            if (rowCount > coverPropertyOffset + coverPropertyCounter)
                                            {
                                                property.Value = coverBenefitsSheet.Rows[coverPropertyOffset + coverPropertyCounter].Cells[col].Value;
                                            }
                                            cover.PropertyList.Add(property);
                                            //add the property to propertyList in product setup object
                                            var productSetupObjectProperty = new PropertyModel(property);
                                            productSetupObjectProperty.Value = "";
                                            if (!productSetupModel.PropertyList.Any(p => p.Code == productSetupObjectProperty.Code))
                                            {
                                                productSetupModel.PropertyList.Add(productSetupObjectProperty);
                                            }
                                        }
                                        coverPropertyOffset += coverPropertyList.Count;
                                        for (int foCoverCodeCounter = 0; foCoverCodeCounter < foCoverCodeList.Count; foCoverCodeCounter++)
                                        {
                                            if (cover.Code == foCoverCodeList[foCoverCodeCounter])
                                            {
                                                cover.CodeBack = boCoverCodeList[foCoverCodeCounter];
                                            }
                                        }
                                        product.CoverList.Add(cover);
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            #endregion

            #region Map productMappingSheet 
            Worksheet productMappingSheet = productSetupWorksheetList.FirstOrDefault(w => w.Name.ToInternalName() == "Product Mapping".ToInternalName());


            var productList = GetColumn("Product Code Front", productMappingSheet);
            var boMappingRuleList = GetColumn("BO Mapping Rule", productMappingSheet);
            var productCodeBackList = GetColumn("Product Code Back", productMappingSheet);
            var boProductStatusList = GetColumn("BO Product Status", productMappingSheet);
            if (
            productList.Count != boMappingRuleList.Count ||
            productList.Count != productCodeBackList.Count ||
            productList.Count != boProductStatusList.Count
            )
            {
                throw new Exception("Product Code Front, BO Mapping Rule, Product Code Back and BO Product Status fields in Product Mapping sheet should have the same values count");
            }



            for (int i = 0; i < productList.Count; i++)
            {
                ////LineOfBusinessGroup
                foreach (var lineOfBusinessGroup in productSetupModel.LineOfBusinessGroupList)
                {
                    //LineOfBusiness
                    foreach (var lineOfBusiness in lineOfBusinessGroup.LineOfBusinessList)
                    {

                        //package
                        foreach (var package in lineOfBusiness.PackageList)
                        {
                            var product = package.ProductList.FirstOrDefault(pac => pac.Code == productList[i]);
                            if (product != null)
                            {
                                ProductMappingModel productMappingModel = new ProductMappingModel();

                                productMappingModel.MappingRule = boMappingRuleList[i];
                                productMappingModel.CodeBack = productCodeBackList[i];
                                productMappingModel.Status = boProductStatusList[i];

                                product.ProductMappingList.Add(productMappingModel);
                            }
                        }

                    }
                }
            }
            #endregion





            return productSetupModel;
        }

        /// <summary>
        /// Retrieve column from worksheet
        /// </summary>
        /// <param name="index">column index</param>
        /// <param name="sheet">shhet</param>
        /// <returns>the selected column (list of string)</returns>
        private List<string> GetColumn(int index, Worksheet sheet)
        {
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Cells[index] != null)
                {
                    column.Add(rows[i].Cells[index].Value);
                }
                else
                {
                    column.Add("");
                }
            }

            return column;
        }

        /// <summary>
        /// Retrieve column from worksheet
        /// </summary>
        /// <param name="fieldTitle">column Title</param>
        /// <param name="sheet">shhet</param>
        /// <returns>the selected column (list of string)</returns>
        private List<string> GetColumn(string fieldTitle, Worksheet sheet)
        {
            int index = -1;
            for (int i = 0; i < sheet.Rows[0].Cells.Count; i++)
            {
                if (sheet.Rows[0].Cells[i] != null)
                {
                    if (sheet.Rows[0].Cells[i].Value.ToInternalName() == fieldTitle.ToInternalName())
                    {
                        index = i;
                    }
                }
            }
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            if (index != -1)
            {
                for (int i = 1; i < rows.Count; i++)
                {
                    if (rows[i].Cells[index] != null)
                    {
                        column.Add(rows[i].Cells[index].Value);
                    }
                    else
                    {
                        column.Add("");
                    }
                }
            }


            return column;
        }

        //todo: needs documentation
        private List<string> GetColumnValues(int index, Worksheet sheet)
        {
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Cells[index] != null)
                {
                    if (!column.Contains(rows[i].Cells[index].Value))
                    {
                        column.Add(rows[i].Cells[index].Value);
                    }
                }
            }

            return column;
        }

        //todo: needs documentation
        private List<string> GetColumnValues(string fieldTitle, Worksheet sheet)
        {
            int index = -1;
            for (int i = 0; i < sheet.Rows[0].Cells.Count; i++)
            {
                if (sheet.Rows[0].Cells[i] != null)
                {
                    if (sheet.Rows[0].Cells[i].Value == fieldTitle)
                    {
                        index = i;
                    }
                }
            }
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            if (index != -1)
            {
                for (int i = 1; i < rows.Count; i++)
                {
                    if (rows[i].Cells[index] != null)
                    {
                        if (!column.Contains(rows[i].Cells[index].Value))
                        {
                            column.Add(rows[i].Cells[index].Value);
                        }
                    }
                }
            }


            return column;
        }


        //todo: needs documentation
        private List<string> GetGroupedColumnNonEmptyValues(string fieldTitle, Worksheet sheet)
        {
            int index = -1;
            for (int i = 0; i < sheet.Rows[0].Cells.Count; i++)
            {
                if (sheet.Rows[0].Cells[i] != null)
                {
                    if (sheet.Rows[0].Cells[i].Value == fieldTitle)
                    {
                        index = i;
                    }
                }
            }
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            if (index != -1)
            {
                for (int i = 1; i < rows.Count; i++)
                {
                    if (rows[i].Cells[index] != null)
                    {
                        if (!column.Contains(rows[i].Cells[index].Value) && !string.IsNullOrEmpty(rows[i].Cells[index].Value))
                        {
                            column.Add(rows[i].Cells[index].Value);
                        }
                    }
                }
            }


            return column;
        }
        //todo: needs documentation
        private List<string> GetGroupedColumnNonEmptyValues(int index, Worksheet sheet)
        {
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Cells[index] != null)
                {
                    if (!column.Contains(rows[i].Cells[index].Value) && !string.IsNullOrEmpty(rows[i].Cells[index].Value))
                    {
                        column.Add(rows[i].Cells[index].Value);
                    }
                }
            }

            return column;
        }

        //todo: needs documentation
        private List<string> GetColumnNonEmptyValues(string fieldTitle, Worksheet sheet)
        {
            int index = -1;
            for (int i = 0; i < sheet.Rows[0].Cells.Count; i++)
            {
                if (sheet.Rows[0].Cells[i] != null)
                {
                    if (sheet.Rows[0].Cells[i].Value == fieldTitle)
                    {
                        index = i;
                    }
                }
            }
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Cells[index] != null)
                {
                    if (!string.IsNullOrEmpty(rows[i].Cells[index].Value))
                    {
                        column.Add(rows[i].Cells[index].Value);
                    }
                }
            }

            return column;
        }
        private List<string> GetColumnNonEmptyValues(int index, Worksheet sheet)
        {
            List<string> column = new List<string>();
            List<Row> rows = sheet.Rows;
            for (int i = 1; i < rows.Count; i++)
            {
                if (rows[i].Cells[index] != null)
                {
                    if (!string.IsNullOrEmpty(rows[i].Cells[index].Value))
                    {
                        column.Add(rows[i].Cells[index].Value);
                    }
                }
            }

            return column;
        }


        #endregion

        #region Public Methods

        /// <summary>
        ///     this method responsible for creating Database Schema and its require the database to be created with name "JIC_ProductSetup"
        /// </summary>
        /// <param name="sqlScript">SQL script from "JIC.Portal\Controllers\ProductSetup\SqlScript\ProductSetupDatabaseModel.sql" ans it's designed using PDM </param>
        public void InstallSchema()
        {
            var productSetupDataBase = new ProductSetupDataBase();
            productSetupDataBase.Install();
        }

        //todo: needs documentation
        public List<ErrorModel> Validate(ProductSetupFileModel productSetupFileModel)
        {
            List<ErrorModel> errorList = ValidateProductSetupFileModel(productSetupFileModel);
            Workbook uploadedFileWorkBook = RetrieveFileWorkbook(productSetupFileModel.FilePath);
            var schemaValidationErrorList = ValidateProductSetupWorkBookSchema(uploadedFileWorkBook);
            errorList.AddRange(schemaValidationErrorList);

            //Map the file to product setup Model
            ProductSetupModel productSetupModel = MapProductSetupWorkbookToProductSetupModel(uploadedFileWorkBook);

            //Validate the Model


            SaveFile(productSetupFileModel);
            return errorList;
        }

        //todo: needs documentation
        public List<ErrorModel> SaveFile(ProductSetupFileModel productSetupFileModel)
        {
            ProductSetupRepositoryObject.DeleteAll();
            List<ErrorModel> errorList = new List<ErrorModel>();
            //List<ErrorModel> errorList = Validate(productSetupFileModel);


            Workbook uploadedFileWorkBook = RetrieveFileWorkbook(productSetupFileModel.FilePath);

            //Map the file to product setup Model
            ProductSetupModel productSetupModelObject = MapProductSetupWorkbookToProductSetupModel(uploadedFileWorkBook);


            /*
            # Save Process with following order
            
            # Map Properties To Entity
            # Save Properties

            Map Groups Entity
            Save Groups


            Map Line Of Business Entity
            Save Line Of Business
            Save Line Of Business Properties

            Map Packages Entity
            Save Packages
            Save Packages Properties

            Map Packages Entity
            Save Products
            Save Products Properties

            Map Products Mapping Entity
            Save Products Mapping
            
            Map Covers Entity
            Save Covers
            Save Covers Properties

            Map Covers Mapping Entity
            Save Cover Mapping

            Map smi Entity
            Save smi
            Save smi Properties

            Map product setup file Entity
            Save product setup file

            Note: make it in transaction scope
            */

            List<CoverEntity> allCoverEntityList = new List<CoverEntity>();
            List<CoverModel> allCoverModelObjectList = new List<CoverModel>();
            List<string> linkedProductCoverProperty= new List<string>();
            List<string> linkedProductMapping = new List<string>();

            #region Save Properties
            //Map Properties To Entity
            var propertyModelList = productSetupModelObject.PropertyList;
            List<PropertyEntity> allPropertyEntityList = ProductSetupMapper.PropertyModelListToPropertyEntityList(propertyModelList);
            //Save Properties
            allPropertyEntityList = this.PropertyRepositoryObject.InsertMany(allPropertyEntityList);
            #endregion

            #region Save Line Of Business Group Groups

            // Map Groups Entity
            List<LineOfBusinessGroupModel> lineOfBusinessGroupModelObjectList = productSetupModelObject.LineOfBusinessGroupList;
            List<LineOfBusinessGroupEntity> lineOfBusinessGroupEntityList = ProductSetupMapper.LineOfBusinessGroupModelListToLineOfBusinessGroupEntityList(lineOfBusinessGroupModelObjectList);
             //save line of business group
             lineOfBusinessGroupEntityList = LineOfBusinessGroupRepositoryObject.InsertMany(lineOfBusinessGroupEntityList);

            #region for each groupe save related line of business
            foreach (var lineOfBusinessGroupModelObject in lineOfBusinessGroupModelObjectList)
            {

                List<LineOfBusinessModel> lineOfBusinessModelObjectList = lineOfBusinessGroupModelObject.LineOfBusinessList;
                var lineOfBusinessGroupEntityObject = lineOfBusinessGroupEntityList.FirstOrDefault(lobg => lobg.Code == lineOfBusinessGroupModelObject.Code);
                if (lineOfBusinessGroupEntityObject != null)
                {
                    // Map line of business Entity
                    List<LineOfBusinessEntity> lineOfBusinessEntityList = ProductSetupMapper.LineOfBusinessModelListToLineOfBusinessEntityList(lineOfBusinessModelObjectList, lineOfBusinessGroupEntityObject.Id);

                    //save line of business group
                    lineOfBusinessEntityList = LineOfBusinessRepositoryObject.InsertMany(lineOfBusinessEntityList);

                    #region  for each line of business save packages and related properties
                    foreach (var lineOfBusinessModelObject in lineOfBusinessModelObjectList)
                    {

                        LineOfBusinessEntity lineOfBusinessEntityObject = lineOfBusinessEntityList.FirstOrDefault(lob => lob.Code == lineOfBusinessModelObject.Code);
                        if (lineOfBusinessEntityObject != null)
                        {
                            #region Save Line Of Business related properties
                            List<PropertyModel> lineOfBusinessPropertyModelObjectList = lineOfBusinessModelObject.PropertyList;
                            List<PropertyEntity> propertyEntityObjectList = new List<PropertyEntity>();

                            foreach (var lineOfBusinessPropertyModelObject in lineOfBusinessPropertyModelObjectList)
                            {
                                PropertyEntity propertyEntityObject = allPropertyEntityList.FirstOrDefault(p => p.Code == lineOfBusinessPropertyModelObject.Code);
                                if (propertyEntityObject != null)
                                {
                                    propertyEntityObject.Value = lineOfBusinessPropertyModelObject.Value;
                                    propertyEntityObjectList.Add(propertyEntityObject);
                                }
                            }
                            foreach (var item in propertyEntityObjectList)
                            {
                                LineOfBusinessRepositoryObject.LinkLineOfBusinessToProperty(lineOfBusinessEntityObject, item);
                            }
                            //LineOfBusinessRepositoryObject.LinkLineOfBusinessToManyProperty(lineOfBusinessEntityObject, propertyEntityObjectList);
                            #endregion

                            #region Save related Packages
                            List<PackageModel> packageModelObjectList = lineOfBusinessModelObject.PackageList;
                            List<PackageEntity> packageEntityObjectList = ProductSetupMapper.PackageModelListToPackageEntityList(packageModelObjectList, lineOfBusinessEntityObject.Id);
                            packageEntityObjectList = PackageRepositoryObject.InsertMany(packageEntityObjectList);


                            #region Collect and Save all products
                            List<ProductEntity> productEntityObjectList = new List<ProductEntity>();

                            foreach (var packageModelObject in packageModelObjectList)
                            {
                                var packageEntityObject = packageEntityObjectList.FirstOrDefault(p => p.Code == packageModelObject.Code);
                                if (packageEntityObject != null)
                                {
                                    List<ProductModel> productModelObjectList = packageModelObject.ProductList;
                                    var productList = ProductSetupMapper.ProductModelListToProductEntityList(productModelObjectList, packageEntityObject.Id);
                                    foreach (var product in productList)
                                    {
                                        if (!productEntityObjectList.Any(p => p.Code == product.Code))
                                        {
                                            productEntityObjectList.Add(product);
                                        }
                                    }
                                }
                           
                            }
                            productEntityObjectList = ProductRepositoryObject.InsertMany(productEntityObjectList);
                            #endregion

                            #region for each Package save related property and product
                 
                            foreach (var packageModelObject in packageModelObjectList)
                            {

                                PackageEntity packageEntityObject = packageEntityObjectList.FirstOrDefault(p => p.Code == packageModelObject.Code);
                                if (packageEntityObject != null)
                                {
                                    #region Save related Package properties
                                    List<PropertyModel> packagePropertyModelObjectList = packageModelObject.PropertyList;
                                    List<PropertyEntity> packagePropertyEntityObjectList = new List<PropertyEntity>();
                                    foreach (var packagePropertyModelObject in packagePropertyModelObjectList)
                                    {
                                        PropertyEntity propertyEntityObject = allPropertyEntityList.FirstOrDefault(p => p.Code == packagePropertyModelObject.Code);

                                        if (propertyEntityObject != null)
                                        {
                                            propertyEntityObject.Value = packagePropertyModelObject.Value;
                                            packagePropertyEntityObjectList.Add(propertyEntityObject);
                                        }
                                    }
                                    foreach (var item in packagePropertyEntityObjectList)
                                    {
                                        PackageRepositoryObject.LinkPackageToProperty(packageEntityObject, item);
                                    }
                                    //PackageRepositoryObject.LinkPackageToManyProperty(packageEntityObject, packagePropertyEntityObjectList);

                                    #endregion

                                    #region Save related Products


                                    List<ProductModel> productModelObjectList = packageModelObject.ProductList;
                                    List<ProductEntity> packageRelatedProductEntityObjectList = new List<ProductEntity>();

                                    foreach (var productModel in productModelObjectList)
                                    {
                                        var productEntity = productEntityObjectList.FirstOrDefault(p => p.Code == productModel.Code);
                                        if (productEntity != null)
                                        {
                                            productEntity.PackageId = packageEntityObject.Id;
                                            PackageRepositoryObject.LinkPackageToProduct(packageEntityObject, productEntity);

                                        }
                                    }


                                  
                                    #region Collect and Save all Covers
                                    foreach (var productModelObject in productModelObjectList)
                                    {
                                        List<CoverModel> productCoverModelObjectList = productModelObject.CoverList;

                                        foreach (var productCover in productCoverModelObjectList)
                                        {
                                            if (!allCoverEntityList.Any(c => c.Code == productCover.Code))
                                            {
                                                //Map cover model list
                                                var cover = ProductSetupMapper.CoverModelToCoverEntity(productCover);
                                                //Save all Cover Model 
                                                var coverEntity = CoverRepositoryObject.Insert(cover);
                                                allCoverEntityList.Add(coverEntity);
                                            }
                                        }
                                    }
                                 
                                    #endregion

                                    #region For Each Product, Save Related Cover And Product Mapping
                                    foreach (var productModelObject in productModelObjectList)
                                    {
                                        ProductEntity productEntityObject = productEntityObjectList.FirstOrDefault(p => p.Code == productModelObject.Code);
                                        if (productEntityObject != null)
                                        {
                                            #region Save Related Covers
                                            List<CoverEntity> productRelatedCoverEntityList = new List<CoverEntity>();
                                            List<CoverModel> productCoverModelObjectList = productModelObject.CoverList;
                                            foreach (var productCoverModel in productCoverModelObjectList)
                                            {
                                                CoverEntity productRelatedCoverEntity = allCoverEntityList.FirstOrDefault(c => c.Code == productCoverModel.Code);
                                                if (productRelatedCoverEntity != null)
                                                {
                                                    foreach (var coverProperty in productCoverModel.PropertyList)
                                                    {
                                                        var propertyEntity = allPropertyEntityList.FirstOrDefault(p => p.Code == coverProperty.Code);

                                                        if (propertyEntity != null)
                                                        {
                                                            //i created list of string to check if the (productEntityObject.Code + productRelatedCoverEntity.Code + propertyEntity.Code) inserted or not
                                                            if (!linkedProductCoverProperty.Any(pcp => pcp == productEntityObject.Code + productRelatedCoverEntity.Code + propertyEntity.Code))
                                                            {
                                                                propertyEntity.Value = coverProperty.Value;
                                                                ProductRepositoryObject.LinkProductToCoverProperty(productEntityObject, productRelatedCoverEntity, propertyEntity);
                                                                linkedProductCoverProperty.Add(productEntityObject.Code + productRelatedCoverEntity.Code + propertyEntity.Code);
                                                            }
                                                          
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Save Product Mapping
                                            List<ProductMappingModel> productMappingModelList = productModelObject.ProductMappingList;
                                            List<ProductMappingEntity> x = ProductSetupMapper.ProductMappingModelListToProductMappingEntityList(productMappingModelList, productEntityObject.Id);
                                            #endregion
                                        }

                                    }
                                    #endregion

                                    #endregion
                                }
                            }
                            #endregion
                            #endregion

                        }

                    }
                    #endregion  
                }


            }
            #endregion
            #endregion
            return errorList;
        }

        //todo: needs documentation
        public List<ErrorModel> Rollback(int version)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();
            if (version < 0)
            {
                errorList.Add(new ErrorModel()
                {
                    Type = ErrorType.ValidationError,
                    Message = "version should not be less than 0"
                });
                return errorList;
            }
            return errorList;
        }
        #endregion
    }
}
