using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Derma.UI.Models;

namespace Derma.UI.ViewModels;

public partial class ConsultasViewModel : ObservableObject
{
    [ObservableProperty]
    private string _textoBusqueda = string.Empty;

    public ObservableCollection<PacienteResumen> Pacientes { get; } = new()
    {
        new PacienteResumen { Nombre = "Carmen Ruíz", Dpi = "1234456701", UltimaConsulta = "15/10/2023", MedicoAsignado = "Dr. Morales" },
        new PacienteResumen { Nombre = "Javier López", Dpi = "1234456704", UltimaConsulta = "15/10/2023", MedicoAsignado = "Dra. Vargas" },
        new PacienteResumen { Nombre = "Lucía Fernández", Dpi = "1233456702", UltimaConsulta = "15/10/2023", MedicoAsignado = "Dr. Morales" },
        new PacienteResumen { Nombre = "Roberto García", Dpi = "1234456780", UltimaConsulta = "15/10/2023", MedicoAsignado = "Dra. Vargas" },
        new PacienteResumen { Nombre = "María Sánchez", Dpi = "1234456790", UltimaConsulta = "12/10/2023", MedicoAsignado = "Dr. Morales" },
    };

    public ICollectionView VistaPacientes { get; }

    public ConsultasViewModel()
    {
        VistaPacientes = CollectionViewSource.GetDefaultView(Pacientes);
        VistaPacientes.Filter = FiltrarPaciente;
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        VistaPacientes.Refresh();
    }

    private bool FiltrarPaciente(object obj)
    {
        if (string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            return true;
        }

        if (obj is not PacienteResumen paciente)
        {
            return false;
        }

        return paciente.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            || paciente.Dpi.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase);
    }

    [RelayCommand]
    private void NuevoPaciente()
    {
        MessageBox.Show("El registro de nuevos pacientes estará disponible próximamente.", "DERMA",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void Consultar(PacienteResumen paciente)
    {
        MessageBox.Show($"La consulta de {paciente.Nombre} estará disponible próximamente.", "DERMA",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    [RelayCommand]
    private void VerHistorial(PacienteResumen paciente)
    {
        MessageBox.Show($"El historial de {paciente.Nombre} estará disponible próximamente.", "DERMA",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
