using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SistemaPaisa.Domain.Entities;
using SistemaPaisa.Infrastructure.Persistence;

namespace SistemaPaisa.Infrastructure.Repositories;

public class SupplierRepository(AppDbContext db) : ISupplierRepository
{
    private static void Log(string operation, object? body) =>
        Console.WriteLine($"[Supplier Repository] {operation}: {JsonSerializer.Serialize(body)}");

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var suppliers = await db.Suppliers.Where(s => s.IsActive).ToListAsync();
        Log("GetAllAsync response", suppliers);
        return suppliers;
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        Log("GetByIdAsync request", new { id });
        var supplier = await db.Suppliers.FindAsync(id);
        Log("GetByIdAsync response", supplier);
        return supplier;
    }

    public async Task AddAsync(Supplier supplier)
    {
        Log("AddAsync request", supplier);
        db.Suppliers.Add(supplier);
        await db.SaveChangesAsync();
        Log("AddAsync response", supplier);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        Log("UpdateAsync request", supplier);
        supplier.UpdatedAt = DateTime.UtcNow;
        db.Suppliers.Update(supplier);
        await db.SaveChangesAsync();
        Log("UpdateAsync response", supplier);
    }

    public async Task DeleteAsync(int id)
    {
        Log("DeleteAsync request", new { id });
        var supplier = await db.Suppliers.FindAsync(id);
        if (supplier is not null)
        {
            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();
        }
        Log("DeleteAsync response", new { id, deleted = supplier is not null });
    }
}
