using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AgroOrganic.Models.MapModels
{
    public class MapDBInitializer : DropCreateDatabaseAlways<MapContext>
    {
        protected override void Seed(MapContext db)
        {
            db.MapLayers.Add(new MapLayer { Name = "pH грунту", Measure = "Од. pH", MinValue = 0, MaxValue = 10 });
            db.MapLayers.Add(new MapLayer { Name = "Органічна речовина", Measure = "%", MinValue = 0, MaxValue = 10 });
            db.MapLayers.Add(new MapLayer { Name = "Нітрати (NO3)", Measure = "мг/кг", MinValue = 0, MaxValue = 20 });
            db.MapLayers.Add(new MapLayer { Name = "Фосфор (P)", Measure = "мг/кг", MinValue = 0, MaxValue = 100 });
            db.MapLayers.Add(new MapLayer { Name = "Калій (K)", Measure = "мг/кг", MinValue = 0, MaxValue = 300 });
            db.MapLayers.Add(new MapLayer { Name = "Кальцій (Ca)", Measure = "мг/кг", MinValue = 0, MaxValue = 10000 });
            db.MapLayers.Add(new MapLayer { Name = "Магній (Mg)", Measure = "мг/кг", MinValue = 0, MaxValue = 1000 });
            db.MapLayers.Add(new MapLayer { Name = "Натрій (Na)", Measure = "мг/кг", MinValue = 0, MaxValue = 100 });
            db.MapLayers.Add(new MapLayer { Name = "Сірка (S)", Measure = "мг/кг", MinValue = 0, MaxValue = 100 });
            db.MapLayers.Add(new MapLayer { Name = "Цинк (Zn)", Measure = "мг/кг", MinValue = 0, MaxValue = 2 });
            db.MapLayers.Add(new MapLayer { Name = "Залізо (Fe)", Measure = "мг/кг", MinValue = 0, MaxValue = 100 });
            db.MapLayers.Add(new MapLayer { Name = "Марганець (Mn)", Measure = "мг/кг", MinValue = 0, MaxValue = 100 });
            db.MapLayers.Add(new MapLayer { Name = "Мідь (Cu)", Measure = "мг/кг", MinValue = 0, MaxValue = 10 });
            db.MapLayers.Add(new MapLayer { Name = "Бор (B)", Measure = "мг/кг", MinValue = 0, MaxValue = 2 });
            base.Seed(db);
        }
    }
}