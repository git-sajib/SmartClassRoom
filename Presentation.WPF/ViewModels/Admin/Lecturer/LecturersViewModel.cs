﻿
using Presentation.ViewModels;
using Presentation.WPF.Commands.Callbcks;
using SmartClassRoom.Domain.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Presentation.Admin.ViewModels
{
    /// <summary>
    /// Class LecturersViewModel
    /// Write your documentation here
    /// </summary>
    public class LecturersViewModel : BaseViewModel
    {
        private readonly ILecturerService lecturserService;
        public ObservableCollection<LecturerListItemViewModel> Items { get; set; } = new ObservableCollection<LecturerListItemViewModel>();
        public LecturerListItemViewModel SelectLecturerItem { get; set; }
        public ICommand RemoveItem { get; set; }

        #region constructor
        public LecturersViewModel(ILecturerService lecturserService)
        {
            this.lecturserService = lecturserService;
            RemoveItem = new RelayACommand(RemoveSelectedItem);
            LoadLecturer();
        }
        #endregion

        private void RemoveSelectedItem()
        {

            if (SelectLecturerItem == null)
            {
                MessageBox.Show("Please Select A Lecturer ", "No Lecturer Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Items.Remove(SelectLecturerItem);
        }

        public async void LoadLecturer() {
            var lecturers = await lecturserService.GetAll();
            foreach (var lecturer in lecturers) {
                Items.Add(new LecturerListItemViewModel {
                    Id = lecturer.Id,
                    Name = lecturer.User.Name,
                    Faculty = lecturer.Faculty,
                    Initials = GetInitials(lecturer.User.Name),
                    ProfilePictureRGB = RandomRGBColor(),
                    CourseCount = lecturer.Sections.Count(),
                    JoinDate = lecturer.User.CreatedAt,
                }); ;
            }
            
        }

        public static string RandomRGBColor()
        {
            Random random = new Random();
            var color = String.Format("{0:X6}", random.Next(0x1000000)); // = "#A197B9"
            return color;
        }

        public static string GetInitials(string anyString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var two = anyString.Split(" ");
            if (two.Length == 2)
            {
                stringBuilder.Append(TruncateLongString(two[0], 1));
                stringBuilder.Append(TruncateLongString(two[1], 1));
            }
            else
            {
                stringBuilder.Append(TruncateLongString(two[0], 2));
            }
            return stringBuilder.ToString().ToUpper();
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
    }
}