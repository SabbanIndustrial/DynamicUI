using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DynamicUI
{
    public class DebugPageBase : ContentPage
    {
        protected bool IsInitialized { get; set; } = false;

        public Image Logo { get; set; }

        public DebugPageViewModel Model { get; set; } = new DebugPageViewModel();
        public DebugPageBase() : base()
        {
            BindingContext = Model;
            BackgroundColor = Color.White;
            Title = GetType().Name;


            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star),
            });

            //grid.ColumnDefinitions.Add(new ColumnDefinition()
            //{
            //    Width = new GridLength(2, GridUnitType.Star),
            //});

            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(1, GridUnitType.Auto),
            });
            grid.RowDefinitions.Add(new RowDefinition()
            {
                //Height = new GridLength(1, GridUnitType.Star),
            });


            AutoScroll.Content = InfoLayout;
            ButtonsLayout.Orientation = StackOrientation.Horizontal;

            ButtonsScroll.Orientation = ScrollOrientation.Horizontal;
            ButtonsScroll.Content = ButtonsLayout;


            grid.Children.Add(ButtonsScroll, 0, 0);
            grid.Children.Add(AutoScroll, 0, 1);


            Logo = new Image()
            {
                Source = "samyy76",
                Opacity = 0.3,

            };


            AbsoluteLayout.Children.Add(Logo);
            AbsoluteLayout.Children.Add(grid, new Rectangle(1, 1, 1, 1), AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(Logo, new Rectangle(0.5, 0.5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(Logo, AbsoluteLayoutFlags.All);
            Content = AbsoluteLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (IsInitialized)
            {
                return;
            }
            AddAction(FabricFontAll.ClearFormatting, () =>
            {
                InfoLayout.Children.Clear();
                WriteString($"Экран очищен");
            });


            foreach (var item in Objects)
            {
                switch (item.UIType)
                {
                    case UIDynType.Button:

                        Button b = new Button
                        {
                            Text = item.Name,
                            BackgroundColor = Colors.White,
                            BorderColor = Colors.Blue,
                            BorderWidth = 1,
                            TextColor = Colors.Gray
                        };
                        if (item.Name.Length == 1)
                        {
                            b.FontSize = 24;
                            b.FontFamily = Fonts.FabricIcons;
                            b.CornerRadius = 16;
                        }
                        b.Clicked += (sender, e) =>
                        {
                            try
                            {
                            //WriteString($"Выполнение: {item.Name}");
                            item.Action?.Invoke();
                            }
                            catch (Exception ex)
                            {
                                WriteString($" '{item.Name}' thrown an exception: ", ex);
                            }
                        };
                        ButtonsLayout.Children.Add(b);


                        break;
                    case UIDynType.Label:
                        Label temp = new Label()
                        {
                            Margin = 10,
                            Text = $"{item.Name}",
                        };
                        ButtonsLayout.Children.Add(temp);
                        break;
                    case UIDynType.View:
                        ButtonsLayout.Children.Add(item.View);
                        break;
                    case UIDynType.Switch:
                        StackLayout stackLayout = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal
                        };

                        if (!string.IsNullOrWhiteSpace(item.Name))
                        {
                            Label temp2 = new Label()
                            {
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                Margin = 5,
                                Text = $"{item.Name}",
                            };
                            if (item.Name.Length == 1)
                            {
                                temp2.FontSize = 24;
                                temp2.FontFamily = Fonts.FabricIcons;
                            }
                            stackLayout.Children.Add(temp2);

                            //ButtonsLayout.Children.Add(temp2);
                        }


                        Frame f = new Frame();
                        f.BorderColor = Colors.Blue;
                        f.CornerRadius = 16;
                        f.HasShadow = false;
                        f.Padding = new Thickness(0);
                        //f.Content =


                        Switch sw = new Switch()
                        {
                            AutomationId = item.Name,
                        };
                        Switches.Add(item.Name, sw);
                        sw.Toggled += (sender, e) =>
                        {
                            Flags[item.Name] = e.Value;
                            item.SwitchAction?.Invoke(e.Value);
                        };
                        stackLayout.Children.Add(sw);
                        f.Content = stackLayout;
                        ButtonsLayout.Children.Add(f);

                        break;
                    case UIDynType.Picker:
                        StackLayout stackLayout2 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal
                        };




                        Frame f2 = new Frame();
                        //f2.BorderColor = Colors.Blue;
                        f2.CornerRadius = 16;
                        f2.HasShadow = false;
                        f2.Padding = new Thickness(0);

                        if (!item.Name.StartsWith('-'))
                        {
                            Label temp3 = new Label()
                            {
                                Margin = new Thickness(5, 5, 5, 0),
                                Text = $"{item.Name.TrimStart('-')}",
                            };
                            stackLayout2.Children.Add(temp3);
                            //ButtonsLayout.Children.Add(temp3);
                        }
                        Picker picker = new Picker()
                        {
                            Margin = new Thickness(5, 0, 5, 5),
                            Title = item.Name.TrimStart('-'),
                        };
                        stackLayout2.Children.Add(picker);

                        foreach (var item2 in item.PickerVariants)
                        {
                            picker.Items.Add(item2);
                        }
                        picker.SelectedIndexChanged += (sender, e) =>
                        {
                            item.PickerAction?.Invoke((string)picker.SelectedItem);
                        };
                        if (item.PickerSelected != null)
                        {
                            picker.SelectedItem = item.PickerSelected;
                        }

                        f2.Content = stackLayout2;
                        ButtonsLayout.Children.Add(f2);

                        break;
                    default:
                        break;
                }

            }


            //foreach (var item in Actions)
            //{
            //    string name = item.Item1;
            //    Action action = item.Item2;

            //    if (action == null)
            //    {
            //        Label temp = new Label()
            //        {
            //            Margin = 10,
            //            Text = $"{name}",
            //        };
            //        ButtonsLayout.Children.Add(temp);
            //        continue;
            //    }

            //    if (name.StartsWith("-"))
            //    {
            //        continue;
            //    }
            //    Button b = new Button
            //    {
            //        Text = name,
            //    };
            //    b.Clicked += (object sender, EventArgs e) =>
            //    {
            //        try
            //        {
            //            WriteString($"Executing {name}");
            //            action.Invoke();
            //        }
            //        catch (Exception ex)
            //        {
            //            WriteString($"Action '{name}' thrown an exception: ", ex);
            //        }
            //    };
            //    ButtonsLayout.Children.Add(b);
            //}
            IsInitialized = true;
        }


        public AbsoluteLayout AbsoluteLayout { get; set; } = new AbsoluteLayout();
        public ScrollView AutoScroll { get; set; } = new ScrollView();
        public StackLayout InfoLayout { get; set; } = new StackLayout();
        public ScrollView ButtonsScroll { get; set; } = new ScrollView();
        public StackLayout ButtonsLayout { get; set; } = new StackLayout();

        protected List<UIDynamicObject> Objects { get; set; } = new List<UIDynamicObject>();
        public Dictionary<string, bool> Flags { get; set; } = new Dictionary<string, bool>();
        public Dictionary<string, Switch> Switches { get; set; } = new Dictionary<string, Switch>();

        public void AddAction(string name, Action action)
        {
            Objects.Add(new UIDynamicObject()
            {
                Action = action,
                Name = name,
                UIType = UIDynType.Button,
            });
            //Actions.Add((name, action));
        }


        public void AddCategory(string name)
        {
            Objects.Add(new UIDynamicObject()
            {
                Name = name,
                UIType = UIDynType.Label,
            });
            //Actions.Add((name, null));
        }

        public void AddSwitch(string name, Action<bool> action)
        {

            Objects.Add(new UIDynamicObject()
            {
                Name = name,
                SwitchAction = action,
                UIType = UIDynType.Switch,
            });
            Flags.TryAdd(name, false);
        }
        public void AddPicker(string name, List<string> variants, string pickerSelected, Action<string> action)
        {
            Objects.Add(new UIDynamicObject()
            {
                Name = name,
                PickerAction = action,
                UIType = UIDynType.Picker,
                PickerVariants = variants,
                PickerSelected = pickerSelected
            });
        }
        public void AddPicker(string name, List<string> variants, Action<string> action)
        {
            Objects.Add(new UIDynamicObject()
            {
                Name = name,
                PickerAction = action,
                UIType = UIDynType.Picker,
                PickerVariants = variants,
            });
        }
        public void Set(string flag, bool value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {


                if (Switches.TryGetValue(flag, out Switch swtch))
                {
                //var swtch = ButtonsLayout.Children.FirstOrDefaultFromMany(p =>
                //{
                //    if (p is IViewContainer<View> vc)
                //    {
                //        return vc.Children;
                //    }
                //    else
                //    {
                //        return Array.Empty<View>();
                //    }
                //},
                //(v) =>
                //{
                //    if (v is Switch s)
                //    {
                //        if (s.AutomationId == flag)
                //        {
                //            return true;
                //        }
                //    }
                //    return false;
                //});

                swtch.IsToggled = value;

                }
            });
        }
        public bool IsSet(string flag)
        {
            Flags.TryGetValue(flag, out bool value);
            return value;
        }
        public void AddLink(string name, Page page)
        {
            Button b = new Button();
            b.Text = name;
            b.Clicked += (sender, e) =>
            {
                try
                {
                    WriteString($"Navigating to {name}");
                    Navigation.PushAsync(page);
                }
                catch (Exception ex)
                {
                    WriteString($"Action '{name}' thrown an exception: ", ex);
                }
            };
            ButtonsLayout.Children.Add(b);
        }

        public void AddView(View view)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                InfoLayout.Children.Add(view);

            });
        }
        public void WriteString(byte[] bytes)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label temp = new Label()
                {
                    Text = $"HEX: \n{bytes.ToHex()}",

                };

                InfoLayout.Children.Add(temp);
                AutoScroll.ScrollToAsync(0, AutoScroll.ContentSize.Height, true);

            });
        }
        public void WriteString(string info)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label temp = new Label()
                {
                    Text = $"{info}",

                };

                InfoLayout.Children.Add(temp);
                AutoScroll.ScrollToAsync(0, AutoScroll.ContentSize.Height, true);

            });
        }
        public void WriteString(string info, Color color)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label temp = new Label()
                {
                    Text = $"{info}",
                    TextColor = color
                };

                InfoLayout.Children.Add(temp);
                AutoScroll.ScrollToAsync(0, AutoScroll.ContentSize.Height, true);

            });
        }
        public void WriteString(List<string> info)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var item in info)
                {
                    WriteString(item);
                }
            });
        }
        public void WriteException(Exception ex, string info = "Ошибка")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                StackLayout s = new StackLayout();
                Button b = new Button()
                {
                    Text = info,
                    TextColor = Color.Red,
                };
                Label temp = new Label()
                {
                    Text = $"{info}\n{ex}",
                    TextColor = Color.Red,
                    IsVisible = false,
                };
                b.Clicked += (object sender, EventArgs e) =>
                {
                    temp.IsVisible = !temp.IsVisible;
                };
                s.Children.Add(b);
                s.Children.Add(temp);
                InfoLayout.Children.Add(s);
            });
        }


        public void Emit(LogEvent logEvent)
        {

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    WriteString(logEvent.RenderMessage(), Color.Chocolate);
                    break;
                case LogEventLevel.Debug:
                    WriteString(logEvent.RenderMessage(), Color.Green);

                    break;
                case LogEventLevel.Information:
                    WriteString(logEvent.RenderMessage(), Color.Black);

                    break;
                case LogEventLevel.Warning:
                    WriteString(logEvent.RenderMessage(), Color.Red);

                    break;
                case LogEventLevel.Fatal:
                    WriteString(logEvent.RenderMessage(), Color.Purple);

                    break;
                default:
                    WriteString(logEvent.RenderMessage());
                    break;
            }
        }
        public void WriteString(string info, Exception ex)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label temp = new Label()
                {
                    Text = $"{info}\n{ex}",
                    TextColor = Color.Red,
                };
                InfoLayout.Children.Add(temp);
            });
        }

        #region Prompts

        public async Task<string> PromptStr(string promptVar = "")
        {
            string result = await DisplayPromptAsync("Ввести строку", $"Назначение: {promptVar}");
            if (result == null)
            {
                return "";
            }
            return result;
        }
        public async Task<int> PromptInt(string promptVar = "")
        {
            string result = await DisplayPromptAsync("Ввести число", $"Назначение: {promptVar}", keyboard: Keyboard.Numeric);
            int amount = int.Parse(result);
            return amount;
        }
        public async Task<byte> PromptByte(string promptVar = "")
        {
            string result = await DisplayPromptAsync("Ввести число", $"Назначение: {promptVar}", keyboard: Keyboard.Numeric);
            byte amount = byte.Parse(result);
            return amount;
        }
        #endregion
    }
    public class DebugPageViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class UIDynamicObject
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public Action<bool> SwitchAction { get; set; }

        public View View { get; set; }

        public UIDynType UIType { get; set; }

        public List<string> PickerVariants { get; set; } = new List<string>();
        public string PickerSelected { get; set; }
        public Action<string> PickerAction;
    }
    public enum UIDynType
    {
        Button,
        Label,
        View,
        Switch,
        Picker,
    }
}
