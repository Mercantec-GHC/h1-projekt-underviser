using OnimeBestofrieeeendo.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnimeBestofrieeeendo.Components.Services;

public class ShopBusinessService
{
    public enum SortOption
    {
        None,
        PriceAsc,
        PriceDesc,
        NameAsc,
        NameDesc,
        RarityAsc,
        RarityDesc
    }

    public List<ShopProductView> SortProducts(List<ShopProductView> products, SortOption sortOption)
    {
        return sortOption switch
        {
            SortOption.PriceAsc   => products.OrderBy(p => p.Price).ToList(),
            SortOption.PriceDesc  => products.OrderByDescending(p => p.Price).ToList(),
            SortOption.NameAsc    => products.OrderBy(p => p.Name).ToList(),
            SortOption.NameDesc   => products.OrderByDescending(p => p.Name).ToList(),
            SortOption.RarityAsc  => products.OrderBy(p => RarityRank(p.Rarity)).ToList(),
            SortOption.RarityDesc => products.OrderByDescending(p => RarityRank(p.Rarity)).ToList(),
            _                     => products
        };
    }

    private int RarityRank(string rarity) => rarity switch
    {
        "Common"    => 0,
        "Uncommon"  => 1,
        "Rare"      => 2,
        "Epic"      => 3,
        "Legendary" => 4,
        _           => 0
    };
}