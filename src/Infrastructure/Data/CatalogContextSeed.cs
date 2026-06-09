using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            var catalogBrandNames = GetPreconfiguredCatalogBrands().Select(brand => brand.Brand).ToArray();
            var existingCatalogBrandNames = await catalogContext.CatalogBrands
                .Where(brand => catalogBrandNames.Contains(brand.Brand))
                .Select(brand => brand.Brand)
                .ToListAsync();

            await catalogContext.CatalogBrands.AddRangeAsync(
                GetPreconfiguredCatalogBrands()
                    .Where(brand => !existingCatalogBrandNames.Contains(brand.Brand)));

            await catalogContext.SaveChangesAsync();

            var catalogTypeNames = GetPreconfiguredCatalogTypes().Select(type => type.Type).ToArray();
            var existingCatalogTypeNames = await catalogContext.CatalogTypes
                .Where(type => catalogTypeNames.Contains(type.Type))
                .Select(type => type.Type)
                .ToListAsync();

            await catalogContext.CatalogTypes.AddRangeAsync(
                GetPreconfiguredCatalogTypes()
                    .Where(type => !existingCatalogTypeNames.Contains(type.Type)));

            await catalogContext.SaveChangesAsync();

            var catalogBrands = await catalogContext.CatalogBrands
                .Where(brand => catalogBrandNames.Contains(brand.Brand))
                .ToDictionaryAsync(brand => brand.Brand, brand => brand.Id);
            var catalogTypes = await catalogContext.CatalogTypes
                .Where(type => catalogTypeNames.Contains(type.Type))
                .ToDictionaryAsync(type => type.Type, type => type.Id);
            var catalogItems = GetPreconfiguredItems(catalogTypes, catalogBrands).ToList();
            var catalogItemNames = catalogItems.Select(item => item.Name).ToArray();
            var existingCatalogItemNames = await catalogContext.CatalogItems
                .Where(item => catalogItemNames.Contains(item.Name))
                .Select(item => item.Name)
                .ToListAsync();

            await catalogContext.CatalogItems.AddRangeAsync(
                catalogItems.Where(item => !existingCatalogItemNames.Contains(item.Name)));

            await catalogContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;
            
            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
            {
                new("Yirgacheffe"),
                new("Sidamo"),
                new("Guji"),
                new("Harrar"),
                new("Limu"),
                new("Wollega"),
                new("Humera")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
            {
                new("Coffee"),
                new("Sesame"),
                new("Pulses"),
                new("Spices")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems(
        IReadOnlyDictionary<string, int> catalogTypes,
        IReadOnlyDictionary<string, int> catalogBrands)
    {
        return new List<CatalogItem>
            {
                new(
                    catalogTypes["Coffee"],
                    catalogBrands["Yirgacheffe"],
                    "Premium single-origin green coffee from the Gedeo Zone. Fully washed, hand-sorted to Grade 1 export standard. Bright floral character with citrus and jasmine notes. Ideal for specialty roasters, hotels, and importers.",
                    "Yirgacheffe Grade 1 Washed Green Coffee",
                    45.00M,
                    "/images/products/1.png"),
                new(
                    catalogTypes["Coffee"],
                    catalogBrands["Sidamo"],
                    "Sun-dried natural coffee from Sidamo highlands. Rich body with berry and stone fruit sweetness. Grade 2 export quality, consistent moisture content. Suitable for roasters seeking bold Ethiopian naturals.",
                    "Sidamo Natural Green Coffee Grade 2",
                    38.00M,
                    "/images/products/2.png"),
                new(
                    catalogTypes["Coffee"],
                    catalogBrands["Guji"],
                    "Specialty-grade natural coffee from the Guji zone of Oromia. Complex fruit-forward cup profile. Hand-harvested and sun-dried on raised beds. A favourite among specialty importers.",
                    "Guji Grade 1 Natural Green Coffee",
                    42.00M,
                    "/images/products/3.png"),
                new(
                    catalogTypes["Coffee"],
                    catalogBrands["Harrar"],
                    "Traditional dry-processed coffee from eastern Ethiopia. Wild and winey cup character with mocha and dark berry tones. Grade 4 export quality. Suitable for blending and espresso roasters.",
                    "Harrar Grade 4 Sun-dried Coffee",
                    32.00M,
                    "/images/products/4.png"),
                new(
                    catalogTypes["Sesame"],
                    catalogBrands["Wollega"],
                    "High-oil-content white sesame seeds from Wollega region. Machine-cleaned and triple-sorted. Low foreign matter, consistent size, and excellent shelf life. Supplied in new PP woven bags.",
                    "Ethiopian White Sesame Seeds (Wollega)",
                    20.00M,
                    "/images/products/5.png"),
                new(
                    catalogTypes["Sesame"],
                    catalogBrands["Humera"],
                    "Premium Humera sesame - Ethiopia's most exported variety. Bold nutty flavour, high oleic content. Machine-cleaned and moisture-controlled to export standards. Ideal for tahini, oil extraction, and confectionery.",
                    "Ethiopian Sesame Seeds (Humera)",
                    22.00M,
                    "/images/products/6.png"),
                new(
                    catalogTypes["Pulses"],
                    catalogBrands["Sidamo"],
                    "Cleaned and polished red split lentils from Ethiopian highlands. Uniform red-orange colour, low foreign matter, consistent cook time. Supplied in 25 kg or 50 kg PP woven bags.",
                    "Red Split Lentils (Misir) Export Grade",
                    18.00M,
                    "/images/products/7.png"),
                new(
                    catalogTypes["Pulses"],
                    catalogBrands["Guji"],
                    "Machine-cleaned yellow split peas from highland Ethiopia. Mild flavour, fast cook time, high protein content. Consistent sizing and low moisture. Suitable for food processors and restaurant supply.",
                    "Yellow Split Peas (Kik) Export Grade",
                    16.00M,
                    "/images/products/8.png"),
                new(
                    catalogTypes["Pulses"],
                    catalogBrands["Harrar"],
                    "Large-count Kabuli chickpeas sorted to Grade A export standard. Creamy texture, even sizing, low breakage. Triple-cleaned and de-stoned. Ideal for food processors, wholesalers, and restaurants.",
                    "Kabuli Chickpeas Large (Grade A)",
                    24.00M,
                    "/images/products/9.png"),
                new(
                    catalogTypes["Spices"],
                    catalogBrands["Yirgacheffe"],
                    "Authentic Ethiopian Berbere blend built on sun-dried red chillies and aromatic spices. Traditionally milled to a fine, even powder. Cleaned and quality-checked before packing. For restaurants, hotels, and retail packers.",
                    "Berbere Spice Blend (Traditional Mill)",
                    30.00M,
                    "/images/products/10.png"),
                new(
                    catalogTypes["Spices"],
                    catalogBrands["Sidamo"],
                    "Fiery Ethiopian chilli blend with cardamom and clove undertones. Finely ground from sun-dried whole chillies. Authentic flavour profile trusted by Ethiopian restaurant chains and food importers across the Gulf.",
                    "Mitmita Chili Powder (Hot Blend)",
                    28.00M,
                    "/images/products/11.png"),
                new(
                    catalogTypes["Spices"],
                    catalogBrands["Limu"],
                    "Single-origin ground Korerima (black cardamom) from the Limu forests of Ethiopia. Earthy, smoky, and aromatic. Used in Ethiopian coffee ceremonies and spice blends. Rare outside East Africa.",
                    "Korerima (Ethiopian Cardamom) Ground",
                    35.00M,
                    "/images/products/12.png")
            };
    }
}
