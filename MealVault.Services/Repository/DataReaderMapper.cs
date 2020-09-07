using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MealVault.Services.Repository
{
    public class DataReaderMapper<T>
    {
        public async Task<List<T>> MapToList(DbDataReader dr, CancellationToken token = default)
        {
            if (dr != null && dr.HasRows)
            {
                var entitytypeof = typeof(T);
                var entities = new List<T>();
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entitytypeof.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

                T newObject = default(T);
                while (await dr.ReadAsync(token))
                {
                    newObject = Activator.CreateInstance<T>();

                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDict[dr.GetName(index).ToUpper()];

                            try
                            {
                                if ((info != null) && info.CanWrite)
                                {
                                    var val = dr.GetValue(index);
                                    info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                                }
                            }
                            catch (Exception ex)
                            {

                            }


                        }
                    }

                    entities.Add(newObject);
                }

                return entities;
            }

            return new List<T>();
        }
    }
}
