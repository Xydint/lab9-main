using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using lab9.Models;

namespace lab9.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            var itemProvider = new ItemProvider();
            CarouselButtonsEnabled = false;
            SelectedImages = new ObservableCollection<Image>();
            var strFolder = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName
                + "\\Assets";
            Items = new ObservableCollection<Item>(itemProvider.GetItems(strFolder));
            SelectDirectory = ReactiveCommand.Create<FileItem, Unit>(
                (file) =>
                {
                    var dir = itemProvider.GetFilesNoRecursion(Path.GetDirectoryName(file.Path));

                    SelectedImages.Clear();
                    foreach (var item in dir)
                    {
                        SelectedImages.Add(new Image((FileItem)item));
                    }
                    for (int i = 0; i < dir.Count; i++)
                    {
                        if (file.Path == dir[i].Path)
                        {
                            ChosenIndex = i;
                            break;
                        }
                    }
                    if (SelectedImages.Count > 1)
                        CarouselButtonsEnabled = true;
                    else
                        CarouselButtonsEnabled = false;
                    return Unit.Default;
                });
            GetFilePath = ReactiveCommand.Create<FileItem, Unit>(
                (file) =>
                {
                    for (int i = 0; i < SelectedImages.Count; i++)
                    {
                        if (SelectedImages[i].Path == file.Path)
                        {
                            ChosenIndex = i;
                            break;
                        }
                    }
                    return Unit.Default;
                });
            Button_Back = ReactiveCommand.Create<Unit, Unit>(
                (unit) =>
                {
                    if (ChosenIndex - 1 < 0)
                        ChosenIndex = SelectedImages.Count - 1;
                    else ChosenIndex--;
                    return unit;
                });
            Button_Next = ReactiveCommand.Create<Unit, Unit>(
                (unit) =>
                {
                    ChosenIndex = (ChosenIndex + 1) % SelectedImages.Count;
                    return unit;
                });
        }

        ObservableCollection<Item> items;
        public ObservableCollection<Item> Items
        {
            get { return items; }
            set { this.RaiseAndSetIfChanged(ref items, value); }
        }
        ObservableCollection<Image> selectedImages;
        public ObservableCollection<Image> SelectedImages
        {
            get { return selectedImages; }
            set { this.RaiseAndSetIfChanged(ref selectedImages, value); }
        }
        bool carouselButtonsEnabled;
        public bool CarouselButtonsEnabled
        {
            get { return carouselButtonsEnabled; }
            set { this.RaiseAndSetIfChanged(ref carouselButtonsEnabled, value); }
        }
        int chosenIndex;
        public int ChosenIndex
        {
            get { return chosenIndex; }
            set { this.RaiseAndSetIfChanged(ref chosenIndex, value); }
        }
        public ReactiveCommand<FileItem, Unit> SelectDirectory { get; }
        public ReactiveCommand<FileItem, Unit> GetFilePath { get; }
        public ReactiveCommand<Unit, Unit> Button_Back { get; }
        public ReactiveCommand<Unit, Unit> Button_Next { get; }
    }
}
