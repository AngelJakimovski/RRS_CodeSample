using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.DAL
{
    public interface IRepositoryFactory
    {
        IJobPositionTypeRepository PositionTypeRepository { get; }

        IConfigurationRepository ConfigurationRepository { get; }

        IReviewerRepository ReviewerRepository { get; }

        IJobApplicationRepository JobApplicationRepository { get; }
    }
}
