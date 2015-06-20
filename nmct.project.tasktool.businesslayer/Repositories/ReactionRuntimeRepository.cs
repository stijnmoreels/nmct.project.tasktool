using nmct.project.tasktool.businesslayer.Context;
using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.models.Reports;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace nmct.project.tasktool.businesslayer.Repositories
{
    // -------------------------------------------------------------------------------------------------------------------------
    // Stijn Moreels - 04.06.2015
    //
    // -------------------------------------------------------------------------------------------------------------------------

    public class ReactionRuntimeRepository : GenericRepository<ReactionRuntime, ApplicationDbContext>, IReactionRuntimeRepository
    {
        public ReactionRuntime GetByCardId(string cardId)
        {
            return context.ReactionRuntimes
                .Where(r => r.TrelloCardId.Equals(cardId))
                .SingleOrDefault<ReactionRuntime>();
        }

        public IEnumerable<ReactionRuntime> GetAllForCampusId(string campusId)
        {
            return context.ReactionRuntimes
                .Where(r => r.TrelloCampus.TrelloIdCampus.Equals(campusId));
        }

        public IEnumerable<ReactionRuntime> GetAllTaskReactionRuntime(DateTime startDate, DateTime endDate)
        {
            context.Configuration.LazyLoadingEnabled = false;
            return (from t in context.ReactionRuntimes.Include(m => m.TrelloMembers).Include(c => c.TrelloCampus) where ((DbFunctions.TruncateTime(t.CardIsDone) >= startDate) && (DbFunctions.TruncateTime(t.CardIsDone) <= endDate)) select t);
        }

        public override IEnumerable<ReactionRuntime> All()
        {
            context.Configuration.LazyLoadingEnabled = false;
            return (from t in context.ReactionRuntimes.Include(m => m.TrelloMembers).Include(c => c.TrelloCampus) select t);
        }

        public override ReactionRuntime Insert(ReactionRuntime entity)
        {
            //context.ReactionRuntimes.Attach(entity);
            
            //entity.TrelloMembers
                //.ForEach(m => context.Entry(m).State = EntityState.Unchanged);


            //var item = context.Entry<List<TrelloMember>>(entity.TrelloMembers);
            //item.State = System.Data.Entity.EntityState.Modified;

            //This is is a must else crash because duplicate keys 
           
            entity.TrelloMembers = new List<TrelloMember>();

            foreach (String mbr in entity.MemberIds)
            {
                entity.TrelloMembers.Add((from t in context.TrelloMembers where t.Id == mbr select t).FirstOrDefault<TrelloMember>());
            }          
            return context.ReactionRuntimes.Add(entity);
        }

        public override void Update(ReactionRuntime entityToUpdate)
        {
           // //entityToUpdate.TrelloMembers
           // //    .ForEach(m => context.Entry(m).State = );
           // ReactionRuntime entity = context.ReactionRuntimes
           //     .Where(r => r.TrelloCardId.Equals(entityToUpdate.TrelloCardId))
           //     .SingleOrDefault<ReactionRuntime>();
            
           // //entityToUpdate.TrelloMembers
           // //    .ForEach(m => context.Entry(m).State = EntityState.Unchanged);
           // entity.CardIsDone = entityToUpdate.CardIsDone;
           //// context.ReactionRuntimes.Attach(entityToUpdate);
           // context.SaveChanges();



            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ReactionRuntime entity = db.ReactionRuntimes
                .Where(r => r.TrelloCardId.Equals(entityToUpdate.TrelloCardId))
                
                .SingleOrDefault<ReactionRuntime>();
                entity.CardIsDone = entityToUpdate.CardIsDone;
                db.SaveChanges();

            }
        }
    }
}
