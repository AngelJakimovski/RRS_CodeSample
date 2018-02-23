using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public class AppConfigurationManager : IConfigurationManager
    {
        private IRepositoryFactory repositoryFactory;

        public AppConfigurationManager(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public T GetValue<T>(string key)
        {
            var entity = repositoryFactory.ConfigurationRepository.List().FirstOrDefault(m => string.Equals(m.Key, key));
            if (entity == null)
                return default(T);

            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            var value = (T)tc.ConvertFromString(null, CultureInfo.InvariantCulture, entity.Value);

            return value;
        }

        public void SetValue<T>(string key, T value)
        {
            var entity = repositoryFactory.ConfigurationRepository.List().FirstOrDefault(m => string.Equals(m.Key, key));
            if (entity == null)
            {
                entity = new Configuration()
                {
                    Key = key,
                    Value = value != null ? value.ToString() : "",
                };
                repositoryFactory.ConfigurationRepository.Add(entity);
            }
            else
            {
                entity.Value = value != null ? value.ToString() : "";
                repositoryFactory.ConfigurationRepository.Update(entity);
            }
        }
    }
}
