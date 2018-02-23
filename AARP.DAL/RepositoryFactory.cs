using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private static IRepositoryFactory instance;
        public static IRepositoryFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new RepositoryFactory();
                return instance;
            }

        }

        private RepositoryFactory()
        {
        }

        public IJobPositionTypeRepository PositionTypeRepository
        {
            get
            {
                return new JobPositionTypeRepository();
            }
        }

        public IConfigurationRepository ConfigurationRepository
        {
            get
            {
                return new AppConfigurationRepository();
            }
        }

        public IReviewerRepository ReviewerRepository
        {
            get
            {
                return new ReviewerRepository();
            }
        }

        public IJobApplicationRepository JobApplicationRepository
        {
            get { return new JobApplicationRepository(); }
        }
    }
}
