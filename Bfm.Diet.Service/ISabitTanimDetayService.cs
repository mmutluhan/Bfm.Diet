using System.Collections.Generic;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.Repository;
using Bfm.Diet.Model;

namespace Bfm.Diet.Service
{
    public interface ISabitTanimDetayService : IRepository<SabitTanimDetay, int>
    {
        [Cache(Lifetime = 300)]
        [Log]
        List<SabitTanimDetay> GetMailSabitleri();
    }
}