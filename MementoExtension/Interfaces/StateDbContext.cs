using System.Threading.Tasks;
using MementoExtension.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_mementable.MementoExtension.Interfaces
{
    public interface StateDbContext
    {
        DbSet<State> States { get; set; }
        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

    }
}