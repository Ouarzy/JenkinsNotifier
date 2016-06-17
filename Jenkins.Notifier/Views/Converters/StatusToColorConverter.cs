namespace Jenkins.Notifier.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using Jenkins.Notifier.Model;

    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (JobStatus)value;

            SolidColorBrush color;
            switch (status)
            {
                 case JobStatus.Success:
                    color = new SolidColorBrush(Colors.Green);
                    break;
                 case JobStatus.Aborted:
                    color = new SolidColorBrush(Colors.Gray);
                    break;
                 case JobStatus.None:
                    color = new SolidColorBrush(Colors.Gray);
                    break;
                 case JobStatus.Unstable:
                    color = new SolidColorBrush(Colors.Yellow);
                    break;
                default:
                    color = new SolidColorBrush(Colors.Red);
                    break;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
