using CommunityToolkit.Mvvm.ComponentModel;

namespace Derma.UI.ViewModels;

public partial class PlaceholderViewModel : ObservableObject
{
    [ObservableProperty]
    private string _titulo;

    [ObservableProperty]
    private string _descripcion;

    public PlaceholderViewModel(string titulo, string descripcion)
    {
        _titulo = titulo;
        _descripcion = descripcion;
    }
}
