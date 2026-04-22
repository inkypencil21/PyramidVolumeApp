using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OffsetPyramidApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public List<PyramidLayer> Layers { get; set; } = new();

    public double TotalVolume { get; set; }

    public void OnGet()
    {
        if (Layers.Count == 0)
        {
            Layers = new List<PyramidLayer>
            {
                new(), new(), new()
            };
        }
    }

    public IActionResult OnPostAddLayer()
    {
        Layers.Add(new PyramidLayer());
        return Page();
    }

    public IActionResult OnPostRemoveLayer(int index)
    {
        if (index >= 0 && index < Layers.Count)
        {
            Layers.RemoveAt(index);
        }

        return Page();
    }

    public IActionResult OnPostCalculate()
    {
        if (!ModelState.IsValid)
            return Page();

        double total = 0;

        foreach (var layer in Layers)
        {
            if (layer.TopLength > layer.BaseLength)
            {
                ModelState.AddModelError("", "Top length cannot be greater than base length.");
                return Page();
            }

            total += GetFrustumVolume(
                layer.BaseLength,
                layer.TopLength,
                layer.Height
            );
        }

        TotalVolume = total;

        return Page();
    }

    private double GetFrustumVolume(double baseLen, double topLen, double height)
    {
        double A1 = baseLen * baseLen;
        double A2 = topLen * topLen;

        return (height / 3.0) * (A1 + A2 + Math.Sqrt(A1 * A2));
    }
}

public class PyramidLayer
{
    [Range(0.0001, double.MaxValue)]
    public double BaseLength { get; set; }

    [Range(0, double.MaxValue)]
    public double TopLength { get; set; }

    [Range(0.0001, double.MaxValue)]
    public double Height { get; set; }

    public double OffsetX { get; set; }
    public double OffsetY { get; set; }
}