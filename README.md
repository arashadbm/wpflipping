# WP Flipping

###Features
- Two sides control which flips across its center in both directions based on horizontal gesture.
- Front template and back templates which you can define easily in xaml.

###Screenshots


###Target platforms
- Windows Phone 8.X Silverlight.

###Usage
- You can use install throught Nuget https://www.nuget.org/packages/wpflipping
- Or Pull the source code and use WPFlipping library or copy the classes inside it.

Here is a snippet of usage of the control.
```
 <ListBox x:Name="FlipList" ItemsSource="{Binding Lists}">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Margin="12,32" >
                            <wpflipping:DoubleSidesControl>
                                <wpflipping:DoubleSidesControl.FrontTemplate>
                                    <DataTemplate>
                                        <StackPanel Background="Yellow">
                                            <TextBlock Text="{Binding Name}" Foreground="Blue" FontSize="48"/>
                                        </StackPanel>
                                    </DataTemplate>
                                </wpflipping:DoubleSidesControl.FrontTemplate>
                                <wpflipping:DoubleSidesControl.BackTemplate>
                                    <DataTemplate>
                                        <StackPanel Background="Green">
                                            <Image Source="{Binding ImgSource}" Height="200" Width="200"/>
                                        </StackPanel>
                                    </DataTemplate>
                                </wpflipping:DoubleSidesControl.BackTemplate>
                            </wpflipping:DoubleSidesControl>
                        </StackPanel>
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
```


###Sample
Pull the source code and run the WPFlippingExample windows phone 8.0 example.

The sample features a simple listbox, each listboxitem has a `DoubleSidesControl` with Front and Back templates.

###Issues
Please report any issue you find or contact if you have enhancements, fixes or ideas.

###License
Microsoft Public License (Ms-PL)
