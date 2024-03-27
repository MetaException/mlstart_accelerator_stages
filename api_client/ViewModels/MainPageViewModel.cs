using apiclient.Model;
using apiclient.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace apiclient.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public class Item
    {
        public ImageSource ItemImageSource { get; set; }
        public FileResult FilePath { get; set; }
        public ImageInfo ImageInfo { get; set; }
    }

    private readonly NetUtils _netUtils;

    [ObservableProperty]
    private bool _isInternetErrorVisible;

    [ObservableProperty]
    private ImageSource _imgSource;

    [ObservableProperty]
    private string _imageWidth;

    [ObservableProperty]
    private string _imageHeight;

    [ObservableProperty]
    private string _imageChannels;

    [ObservableProperty]
    private ObservableCollection<Item> _imgs = new ObservableCollection<Item>();

    [ObservableProperty]
    private Item _selectedItem;

    [ObservableProperty]
    private bool _isUploadButtonVisible = false;

    [ObservableProperty]
    private bool _isImageDetailsVisible = false;

    public RelayCommand UploadButtonClickedCommand { get; }

    public RelayCommand OpenFileCommand { get; }

    public RelayCommand<Item> SelectionChangedCommand { get; }

    public MainPageViewModel(NetUtils netUtils)
    {
        _netUtils = netUtils;

        UploadButtonClickedCommand = new RelayCommand(async () => await UploadButtonClicked());
        OpenFileCommand = new RelayCommand(async () => await OpenFile());
        SelectionChangedCommand = new RelayCommand<Item>(async (item) => await SelectionChangedHandler(item));
    }

    private async Task SelectionChangedHandler(Item item)
    {
        ImgSource = ImageSource.FromFile(item.FilePath.FullPath);

        if (IsImageDetailsVisible = item.ImageInfo is not null)
        {
            ImageWidth = $"Ширина: {item.ImageInfo.width}";
            ImageHeight = $"Высота: {item.ImageInfo.height}";
            ImageChannels = $"Количество каналов: {item.ImageInfo.channels}";
        }
    }

    private async Task OpenFile()
    {
        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Выбирите изображения",
            FileTypes = FilePickerFileType.Images
        });

        if (!results.Any())
            return;

        ImgSource = ImageSource.FromFile(results.First().FullPath);

        foreach (var file in results)
        {
            Imgs.Add(new Item { ItemImageSource = ImageSource.FromFile(file.FullPath), FilePath = file });
        }

        SelectedItem = Imgs.Last();

        IsUploadButtonVisible = true;
    }

    private async Task UploadButtonClicked()
    {
        if (SelectedItem is null)
            return;

        var result = SelectedItem.FilePath;

        var fileStream = await result.OpenReadAsync();

        var details = await _netUtils.SendImageAsync(fileStream, result.FileName);

        ImageWidth = $"Ширина: {details.width}";
        ImageHeight = $"Высота: {details.height}";
        ImageChannels = $"Количество каналов: {details.channels}";

        SelectedItem.ImageInfo = details;
        IsImageDetailsVisible = true;
    }
}