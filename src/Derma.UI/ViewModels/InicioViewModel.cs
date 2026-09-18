using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Derma.UI.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Derma.UI.ViewModels;

public partial class InicioViewModel : ObservableObject
{
    [ObservableProperty]
    private string _nombreUsuario;

    [ObservableProperty]
    private string _fechaActual = DateTime.Now.ToString("dddd, d 'de' MMMM yyyy",
        new System.Globalization.CultureInfo("es-ES"));

    public ObservableCollection<CitaProgramada> CitasDeHoy { get; } = new()
    {
        new CitaProgramada { Hora = "09:00 AM", Paciente = "Carmen Ruíz", Medico = "Dr. Morales", Tipo = "Consulta", Estado = "Confirmado" },
        new CitaProgramada { Hora = "10:30 AM", Paciente = "Javier López", Medico = "Dra. Vargas", Tipo = "Control", Estado = "Pendiente" },
        new CitaProgramada { Hora = "11:45 AM", Paciente = "Lucía Fernández", Medico = "Dr. Morales", Tipo = "Procedimiento", Estado = "Confirmado" },
    };

    public ISeries[] SeriesCitasPorMes { get; set; }
    public Axis[] EjesCitasPorMes { get; set; }

    public ISeries[] SeriesTratamientos { get; set; }

    public InicioViewModel(string nombreUsuario)
    {
        _nombreUsuario = nombreUsuario;

        var meses = new[] { "Enero", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sept", "Oct" };
        var valores = new double[] { 17, 15, 11, 22, 18, 16, 15, 20, 23, 24 };

        SeriesCitasPorMes = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = valores,
                Fill = new SolidColorPaint(SKColor.Parse("#2F8F7C")),
                MaxBarWidth = 24
            }
        };

        EjesCitasPorMes = new Axis[]
        {
            new Axis { Labels = meses, LabelsRotation = 0, TextSize = 11 }
        };

        SeriesTratamientos = new ISeries[]
        {
            new PieSeries<double> { Values = new double[] { 35 }, Name = "Limpieza Facial", Fill = new SolidColorPaint(SKColor.Parse("#2F8F7C")) },
            new PieSeries<double> { Values = new double[] { 25 }, Name = "Botox", Fill = new SolidColorPaint(SKColor.Parse("#5AB3A0")) },
            new PieSeries<double> { Values = new double[] { 40 }, Name = "Acné", Fill = new SolidColorPaint(SKColor.Parse("#A8D8CC")) },
        };
    }
}
