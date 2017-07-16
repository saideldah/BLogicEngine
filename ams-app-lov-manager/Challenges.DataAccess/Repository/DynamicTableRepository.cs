using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insight.Database;
using Insight.Database.Providers;
using BSynchro.Web.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Challenges.DataAccess
{

    public abstract class DynamicTableRepository : NonEntityRepository<DynamicTableEntity, Guid>
    {
        public string DynamicTableName { get; private set; }

        public List<string> DynamicFields { get; private set; }

        public List<string> RequiredColumns { get; private set; }
        public bool IsTableExist
        {
            get
            {
                string query = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = '"
                    + this.DynamicTableName 
                    + "')   select 1 as IsTableExist ELSE select 0 as IsTableExist ";
                List<dynamic> list = base.DB.Query<dynamic>(query, null, CommandType.Text).ToList();
                if (list[0].IsTableExist == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public static DynamicTableRepository CreateInstance(string TableName, List<string> dynamicFields)
        {
            DynamicTableRepository repository = RepositoryFactory.Create<DynamicTableRepository>("ChallengesAppDB");
            repository.DynamicTableName = TableName;
            repository.DynamicFields = dynamicFields;
            repository.RequiredColumns = new List<string>()
            {
                "RegionalCode",
                "AgencyCode",
                "AgentCode",
                "AgentName",
                "IntermediaryCode",
                "CancellationPoint"
            };
            return repository;
        }
        public void CreateTable()
        {
            //to do ceck if the name is valid name for column or not
            string query = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = '" + this.DynamicTableName + "')"
                + " BEGIN"
                + "  CREATE TABLE " + this.DynamicTableName
                + "(Id uniqueidentifier not null, ";
            //string.Join(", ", this.RequiredColumns) + ", "
            foreach (var col in this.RequiredColumns)
            {
                query += col + " nvarchar(100),";
            }
            List<string> colList = new List<string>();
            for (int i = 0; i < this.DynamicFields.Count; i++)
            {
                colList.Add(this.DynamicFields[i].ToLower().Replace(" ", "") + " numeric(18,6)");
            }
            query += string.Join(", ", colList);
            query += ", constraint PK_" + this.DynamicTableName + " primary key clustered(Id)) END";
            base.DB.Execute(query, null, CommandType.Text);
        }

        public void DropTable()
        {
            //to do ceck if the name is valid name for column or not
            string query = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = '" + this.DynamicTableName + "')"
             + " BEGIN"
             + "  DROP TABLE " + this.DynamicTableName
             + " END";
            base.DB.Execute(query, null, CommandType.Text);
        }
        public override IList<DynamicTableEntity> SelectAll()
        {
            List<DynamicTableEntity> dynamicTableEntityList = new List<DynamicTableEntity>();
            List<dynamic> list = base.DB.Query<dynamic>("select * from " + this.DynamicTableName, null, CommandType.Text).ToList();

            foreach (var item in list)
            {
                IDictionary<string, object> propertyValues = (IDictionary<string, object>)item;
                DynamicTableEntity dynamicTableEntity = new DynamicTableEntity();
                dynamicTableEntity.Id = new Guid(propertyValues["Id"].ToString());
                dynamicTableEntity.IntermediaryCode = propertyValues["IntermediaryCode"].ToString();
                dynamicTableEntity.AgencyCode = propertyValues["AgencyCode"].ToString();
                dynamicTableEntity.AgentCode = propertyValues["AgentCode"].ToString();
                dynamicTableEntity.AgentName = propertyValues["AgentName"].ToString();
                dynamicTableEntity.RegionalCode = propertyValues["RegionalCode"].ToString();
                if (string.IsNullOrEmpty(propertyValues["CancellationPoint"].ToString()))
                {
                    dynamicTableEntity.CancellationPoint = 0;
                }
                else
                {
                    dynamicTableEntity.CancellationPoint = Convert.ToDouble(propertyValues["CancellationPoint"]);
                }
                foreach (var field in this.DynamicFields)
                {
                    dynamicTableEntity.DynamicFields.Add(field, Convert.ToDouble(propertyValues[field].ToString()));
                }
                dynamicTableEntityList.Add(dynamicTableEntity);
            }
            return dynamicTableEntityList;
        }

        public override Guid Insert(DynamicTableEntity entry)
        {
            entry.Id = Guid.NewGuid();
            string query = "insert into " + this.DynamicTableName + "(Id," + string.Join(", ", this.RequiredColumns) + ", ";
            //to do ceck if the keys is valid or not
            List<string> colList = new List<string>();
            query += string.Join(", ", entry.DynamicFields.Keys);
            query += ") VALUES ('" + entry.Id
                + "', '" + entry.RegionalCode
                + "', '" + entry.AgencyCode 
                + "', '" + entry.AgentCode 
                + "', '" + entry.AgentName 
                + "', '" + entry.IntermediaryCode
                + "', '" + entry.CancellationPoint
                + "' ,"
                ;
            query += string.Join(", ", entry.DynamicFields.Values);
            query += ");";
            base.DB.Execute(query, null, CommandType.Text);

            return entry.Id;
        }
        public void Delete(Guid id)
        {
          
            string query = "DELETE FROM " + this.DynamicTableName + " WHERE Id = '" + id.ToString() + "'";
            base.DB.Execute(query, null, CommandType.Text);
        }

        public DynamicTableEntity Select(Guid id)
        {
            dynamic obj = base.DB.Query<dynamic>("select * from " + this.DynamicTableName + " WHERE Id = '" + id.ToString() + "'" , null, CommandType.Text).ToList().FirstOrDefault();
            if (obj == null)
            {
                return null;
            }
            else
            {
                IDictionary<string, object> propertyValues = (IDictionary<string, object>)obj;

                DynamicTableEntity dynamicTableEntity = new DynamicTableEntity();
                dynamicTableEntity.Id = new Guid(propertyValues["Id"].ToString());
                dynamicTableEntity.IntermediaryCode = propertyValues["IntermediaryCode"].ToString();
                dynamicTableEntity.AgencyCode = propertyValues["AgencyCode"].ToString();
                dynamicTableEntity.AgentCode = propertyValues["AgentCode"].ToString();
                dynamicTableEntity.AgentName = propertyValues["AgentName"].ToString();
                dynamicTableEntity.RegionalCode = propertyValues["RegionalCode"].ToString();
                dynamicTableEntity.CancellationPoint = Convert.ToDouble(propertyValues["CancellationPoint"].ToString());
                foreach (var field in this.DynamicFields)
                {
                    dynamicTableEntity.DynamicFields.Add(field, Convert.ToDouble(propertyValues[field].ToString()));
                }
                return dynamicTableEntity;
            }

        }

        public void Update(DynamicTableEntity entry)
        {
            entry.Id = Guid.NewGuid();
            string query = "UPDATE " + this.DynamicTableName + " SET "
                + "RegionalCode = '" + entry.RegionalCode + "', "
                + "AgencyCode = '" + entry.AgencyCode + "', "
                + "AgentCode = '" + entry.AgentCode + "', "
                + "AgentName = '" + entry.AgentName + "', "
                ;
            foreach (var field in entry.DynamicFields)
            {
                query += field.Key + " = '"+ field.Value +"' ,";
            }
            query += "IntermediaryCode = '" + entry.IntermediaryCode + "' WHERE Id = '"+entry.Id.ToString()+"'";
            base.DB.Execute(query, null, CommandType.Text);

        }

        public List<DynamicTableEntity> InsertMany(List<DynamicTableEntity> entities)
        {
            foreach (var item in entities)
            {
                item.Id = this.Insert(item);
            }
            return entities;
        }
    }
}
