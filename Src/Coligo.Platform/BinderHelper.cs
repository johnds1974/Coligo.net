using Coligo.Core;
using Coligo.Platform;
using Coligo.Platform.Extensions;
using Coligo.Platform.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#if WINDOWS_PHONE_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Xaml.Interactions.Core;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
#endif

using Coligo.Platform.Converters;
using System.Diagnostics;

namespace Coligo.Platform.Binder
{
    public static class BinderHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fe"></param>
        /// <param name="model"></param>
        public static void BindModel(FrameworkElement fe, object model)
        {
            if (fe != null)
            {
                // set the elements DataContext to be the model object...
                fe.DataContext = model;

                return;
/*
                Stack<FrameworkElement> childrenStack = new Stack<FrameworkElement>();

                // Stackup the children...
                childrenStack = GetChildElements(fe);

                while (childrenStack.Count > 0)
                {
                    var child = childrenStack.Pop();

                    if (child is FrameworkElement)
                    {
                        var childelem = child as FrameworkElement;

                        if (childelem.IsLoaded() && childelem.DataContext != model)
                        {
                            childelem.DataContext = model;

                            // Stackup the children...
                            childrenStack = GetChildElements(childelem, childrenStack);
                        }
                        else
                        {
                            childelem.Loaded += (sender, args) =>
                            {
                                var element = sender as FrameworkElement;

                                if (element != null && element.DataContext != model)
                                {
                                    element.DataContext = model;

                                    // Stackup the children...
                                    childrenStack = GetChildElements(childelem, childrenStack);
                                }

                            };
                        }

                    }
                }
*/
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="currentStack"></param>
        /// <returns></returns>
        private static Stack<FrameworkElement> GetChildElements(FrameworkElement element, Stack<FrameworkElement> currentStack = null)
        {
            Stack<FrameworkElement> children = currentStack != null ? new Stack<FrameworkElement>(currentStack) : new Stack<FrameworkElement>();

            var cnt = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < cnt; i++)
            {
                var elem = VisualTreeHelper.GetChild(element, i);
                if (elem is FrameworkElement)
                    children.Push(elem as FrameworkElement);
            }

            return children;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="statement"></param>
        public static void BindElement(FrameworkElement element, string statement)
        {
//            Debug.WriteLine("==>> BindElement - {0} '{1}'", element.Name, statement);

            if (!string.IsNullOrEmpty(statement))
            {
                if (element.DataContext != null)
                {
                    BindElementStatement(element, statement);
                }
                else
                {
                    element.DataContextChanged += (sender, args) =>
                        {
                            var targetElement = sender as FrameworkElement;

//                            Debug.WriteLine("==>> BindElement(DataContextChanged) - {0} '{1}'", targetElement, statement);

                            if (targetElement != null && targetElement.DataContext != null)
                            {
                                BindElementStatement(targetElement, statement);
                            }
                        };
                }
            }
        }

        /// <summary>
        /// Attempt to hookup Conventions to the Model object (DataContext) of the FrameworkElement,
        /// and based on the element type, and it's Name property.
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="statement"></param>
        private static void ApplyThisBinding(FrameworkElement targetElement, string statement)
        {
            var elementName = targetElement.Name;
            if (string.IsNullOrEmpty(elementName))
            {
                if (statement.Contains(":"))
                {
                    var split = statement.Split(new[] {':'});
                    elementName = split[1];
                }
            }

            var targetModel = targetElement.DataContext;

            if (!string.IsNullOrEmpty(elementName) && targetModel != null)
            {
                if (targetElement is Control)
                {
                    #region CONTROL
                    var control = targetElement as Control;

                    // hookup IsEnabledProperty...
                    BindingOperations.SetBinding(control, Control.IsEnabledProperty, new Binding
                    {
                        Path = new PropertyPath("Can" + elementName),
                    });

                    // hookup VisibilityProperty...
                    BindingOperations.SetBinding(control, Control.VisibilityProperty, new Binding
                    {
                        Path = new PropertyPath("IsVisible" + elementName),
                        Converter = new BoolToVisibilityConverter()
                    });
                    #endregion
                }

                if (targetElement is TextBlock)
                {
                    #region TEXTBLOCK

                    // Hookup TextProperty...
#if !WINDOWS_PHONE_APP
//                    BindingOperations.ClearAllBindings(targetElement);
#endif
                    BindingOperations.SetBinding(targetElement, TextBlock.TextProperty, new Binding
                    {
                        Path = new PropertyPath(elementName),
                        Mode = BindingMode.OneWay
                    });


                    #endregion
                }
                else if (targetElement is TextBox)
                {
                    #region TEXTBOX

                    // Hookup TextProperty...
#if !WINDOWS_PHONE_APP
//                    BindingOperations.ClearAllBindings(targetElement);
#endif
                    BindingOperations.SetBinding(targetElement, TextBox.TextProperty, new Binding
                        {
                            Path = new PropertyPath(elementName),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        });

                    #endregion
                }
                else if (targetElement is ButtonBase)
                {
                    #region BUTTONBASE

                    var buttonBase = targetElement as ButtonBase;

#if WINDOWS_PHONE_APP
                    var trigger = new EventTriggerBehavior
                    {
                        EventName = "Click",
                    };
                    trigger.Actions.Add(new InvokeAction(elementName));
#else
                    // Attach a default 'Click' event to an action on the DataContext...
                    var trigger = new System.Windows.Interactivity.EventTrigger
                    {
                        EventName = "Click"
                    };
                    trigger.Actions.Add(new InvokeAction(elementName));
#endif

                    if (buttonBase is ToggleButton)
                    {
                        var togglebutton = buttonBase as ToggleButton;

                        // Hookup IsCheckedProperty...
                        BindingOperations.SetBinding(togglebutton, ToggleButton.IsCheckedProperty, new Binding
                        {
                            Path = new PropertyPath(elementName),
                        });
                    }

                    #endregion
                }
                else if (targetElement is Selector)
                {
                    #region SELECTOR
                    var selector = targetElement as Selector;

                    BindingOperations.SetBinding(selector, Selector.SelectedItemProperty, new Binding 
                    { 
                    });

                    #endregion
                }
            }
            else
            {
                //@TODO: Report a problem here
            }
        }

        /// <summary>
        /// Binds the <paramref name="targetElement"/> to the items in the <paramref name="statement"/> string.
        /// <para>
        /// The statement string can take the form: 
        /// </para>
        /// <para>
        /// "$this" - uses the Name of the control to bind to certain ViewModel properties.
        /// </para>
        /// <para>
        /// "$this:vm-property-name"
        /// </para>
        /// <para>
        /// "control-property-name:vm-property-name"
        /// </para>
        /// 
        /// </summary>
        /// <param name="targetElement"></param>
        /// <param name="statement"></param>
        private static void BindElementStatement(FrameworkElement targetElement, string statement)
        {
            if (targetElement == null)
                throw new ArgumentNullException("targetElement");

            if(string.IsNullOrEmpty(statement))
                throw new ArgumentNullException("statement");

            Debug.WriteLine("==>> BindElementStatement - {0} '{1}' -> '{2}'", targetElement, targetElement.Name, statement);


            // Split up our statement into parts...
            // eg: '$this:Name;<control-prop>:<vm-prop>'

            string[] items = null;
            if (statement.Contains(";"))
                items = statement.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            else
                items = new string[] { statement };

            // Check if '$this' has been specified, which means we will attempt to apply 'conventions' to this element...
            var applyConventions = items.Any(s => s.Equals("$this", StringComparison.CurrentCultureIgnoreCase));

            // Remove the '$this' item from the list, we don't need it anymore...
//            items = items.Where(s => !s.Equals("$this", StringComparison.CurrentCultureIgnoreCase)).ToArray();

            foreach (var part in items)
            {
                // split each 'part' into key/value pairs...
                var kvp = part.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                var key = kvp[0];
                var keyIsThis = key.Equals("$this", StringComparison.CurrentCultureIgnoreCase) ? true : false;
                var keyValue = kvp.Length == 2 ? kvp[1] : null;

                // Lookup the DependencyProperty of the target element...
                if (!keyIsThis)
                {
                    var depprop = targetElement.GetDependencyProperty(key);
#if !WINDOWS_PHONE_APP
                    if (depprop != null && !depprop.ReadOnly)
                    {
                        // Ok, now make a Binding to the VM prop...
                        BindingOperations.SetBinding(targetElement, depprop, new Binding
                        {
                            Path = new PropertyPath(keyValue),
                        });
                    }
#endif
                }

                if (targetElement is Control)
                {
                    #region CONTROL
                    var control = targetElement as Control;

                    if (applyConventions)
                    {
                        // hookup IsEnabledProperty...
                        BindingOperations.SetBinding(control, Control.IsEnabledProperty, new Binding
                        {
                            Path = new PropertyPath("Can" + control.Name),
                        });

                        // hookup VisibilityProperty...
                        BindingOperations.SetBinding(control, Control.VisibilityProperty, new Binding
                        {
                            Path = new PropertyPath("IsVisible" + control.Name),
                            Converter = new BoolToVisibilityConverter()
                        });
                    }
                    #endregion
                }

                if (targetElement is TextBlock)
                {
                    #region TEXTBLOCK

                    // Hookup TextProperty...
#if WINDOWS_PHONE_APP
#endif
                    #region Apply Conventions

                    if (keyIsThis && keyValue != null)
                    {
                        BindingOperations.SetBinding(targetElement, TextBlock.TextProperty, new Binding
                        {
                            Path = new PropertyPath(keyValue),
                            Mode = BindingMode.OneWay
                        });
                    }

                    #endregion

                    if (!keyIsThis)
                    {
                        // The Key needs to be the name of a DependencyProperty on the TargetElement...
                        var depProp = targetElement.GetDependencyProperty(key);

                        BindingOperations.SetBinding(targetElement, depProp, new Binding
                        {
                            Path = new PropertyPath(keyValue),
                            Mode = BindingMode.OneWay
                        });
                    }

                    #endregion
                }
                else if (targetElement is TextBox)
                {
                    #region TEXTBOX

                    // Hookup TextProperty...
#if !WINDOWS_PHONE_APP
//                    BindingOperations.ClearAllBindings(targetElement);
#endif
                    if (applyConventions)
                    {
                        BindingOperations.SetBinding(targetElement, TextBox.TextProperty, new Binding
                        {
                            Path = new PropertyPath(targetElement.Name),
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    }

                    #endregion
                }
                else if (targetElement is ButtonBase)
                {
                    #region BUTTONBASE

                    var buttonBase = targetElement as ButtonBase;

                    Debug.WriteLine(" ==>> Binding 'Click' event to {0} '{1}' with VM {2}...", targetElement, targetElement.Name, targetElement.DataContext);

                    if (applyConventions)
                    {
#if WINDOWS_PHONE_APP
                        var trigger = new EventTriggerBehavior
                        {
                            EventName = "Click",
                        };
                        trigger.Actions.Add(new InvokeAction(targetElement.Name));
#else
                        // Attach a default 'Click' event to an action on the DataContext...
                        var trigger = new System.Windows.Interactivity.EventTrigger
                        {
                            EventName = "Click"
                        };
                        trigger.Actions.Add(new InvokeAction(targetElement.Name));
#endif

                        trigger.Attach(targetElement);
                    }

                    if (buttonBase is ToggleButton)
                    {
                        var togglebutton = buttonBase as ToggleButton;

                        if (applyConventions)
                        {
                            // Hookup IsCheckedProperty...
                            BindingOperations.SetBinding(togglebutton, ToggleButton.IsCheckedProperty, new Binding
                            {
                                Path = new PropertyPath(targetElement.Name),
                            });
                        }
                    }

                    #endregion
                }
                else if (targetElement is ItemsControl)
                {
                    #region SELECTOR
                    var itemsControl = targetElement as ItemsControl;

                    if(!keyIsThis)
                    {

                        var depProp = targetElement.GetDependencyProperty(key);

                        var elementProp = targetElement.GetType().GetProperty(key);

                        var viewModelProp = targetElement.DataContext.GetType().GetProperty(keyValue);

                        // If the element property is a DependencyProperty, then we need to create a Binding...
                        if (viewModelProp != null && depProp != null)
                        {
                            if (elementProp != null && elementProp.CanWrite)
                            {
                                BindingOperations.SetBinding(targetElement, depProp, new Binding
                                {
                                    Path = new PropertyPath(viewModelProp.Name),
                                    Mode = BindingMode.TwoWay
                                });
                            }
                            else
                            {
                                // Ok, so we cannot bind to a property that has no SETTER, so how can we be 'notified' of when that property
                                // changes? Well, like in the case of TreeView, which has a read-only SelectedItem dependency-property, so
                                // we cannot bind to that property, but it does have a SelectedItemChanged event, which we CAN use as a means
                                // of 'hooking' into the fact that the property itself has changed, which we can propogate down to the ViewModel property...
                                var eventInfo = targetElement.GetEvent(key);
                                if (eventInfo != null)
                                {
#if WINDOWS_PHONE_APP
                                    // No items control on WinRT
#else
                                // Attach a default 'Click' event to an action on the DataContext...
                                var trigger = new System.Windows.Interactivity.EventTrigger
                                {
                                    EventName = eventInfo.Name
                                };
                                trigger.Actions.Add(new PropertyInvokeAction(viewModelProp.Name));
                                trigger.Attach(targetElement);
#endif


                                }
                            }
                        }

                        // Now we need to find the FIRST items as a Property on the DataContext...

                        if (viewModelProp != null && elementProp != null)
                        {

                            object modelPropValue = viewModelProp.GetValue(targetElement.DataContext, null);

                            // If the property type is a ViewModel, or a Collection of ViewModel, then
                            // we need to create the appropriate View to attach to the TARGET item property...
                            if (modelPropValue is string)
                            {
                                elementProp.SetValue(targetElement, modelPropValue);
                            }
                            else if (modelPropValue is BaseViewModel)
                            {
                                #region Property is a BaseViewModel...
                                #endregion
                            }
                            else if (modelPropValue is IEnumerable)
                            {
                                #region Property is an IEnumerable...
                                IList<object> targetItems = new List<object>();

                                foreach (var itemValue in modelPropValue as IEnumerable)
                                {
                                    if (itemValue is Type &&
                                        ((Type)itemValue).IsSubType(typeof(BaseViewModel)))
                                    {
                                        // Ok, we have a ViewModel TYPE, now we must Create an instance of it,
                                        // and Create an instance of its View class,
                                        // and Bind them together
                                        var modelType = itemValue as Type;

                                        // Create an instance of the ViewModel...
                                        var viewModel = ColigoEngine.Container.GetInstance(modelType);

                                        // Find the Type for the associated View...
                                        var view = ViewHelper.GetViewForModel(modelType);

                                        if (view != null)
                                        {
                                            // Bind the Target Element with the ViewModel instance...
                                            BindModel(view, viewModel);
                                        }

                                        targetItems.Add(view);
                                    }
                                    else
                                    {
                                        targetItems.Add(itemValue);
                                    }
                                }

                                // Next, we need to magically 'bind' the next item (target element property)...
                                object targetPropValue = elementProp.GetValue(targetElement, null);
                                Type targetPropType = elementProp.PropertyType;

#if WINDOWS_PHONE_APP
                                if (targetElement is Hub)
                                {
                                    if (targetElement.Name == "Sections")
                                    {
                                        foreach (var targetItem in targetItems)
                                        {
                                            ((Hub)targetElement).Sections.Add(targetItem as HubSection);
                                        }
                                    }
                                }
#endif

                                if (targetPropType.IsSubType(typeof(IList)))
                                {
                                    foreach (var targetItem in targetItems)
                                    {
                                        ((IList)targetPropValue).Add(targetItem);
                                    }
                                }

                                if (targetElement is Selector)
                                {
                                    ((Selector)targetElement).SelectedIndex = 0;
                                }

                                #endregion
                            }
                        }
                    }
                    #endregion
                }
#if WINDOWS_PHONE_APP
                else if (targetElement is Hub)
                {
                    if (keyIsThis)
                    {
                        var trigger = new EventTriggerBehavior
                        {
                            EventName = "SectionsInViewChanged",
                        };
                        trigger.Actions.Add(new InvokeAction((s, p, vm) => 
                        {
                            Hub hub = s as Hub;
                            if (hub != null)
                            {
                                var section = hub.SectionsInView[0];
                            }
                        }
                        ));
                        trigger.Attach(targetElement);
                    }
                }
#endif

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="statement"></param>
        public static void BindAction(DependencyObject element, string statement)
        {
            if (element != null && !string.IsNullOrEmpty(statement))
            {
                var split = statement.Split(new [] {':'});

                if(split.Length == 2) {
                    var eventinfo = element.GetType().GetEventInfo(split[0]);
                    if (eventinfo != null)
                    {
#if WINDOWS_PHONE_APP
                        var trigger = new EventTriggerBehavior
                        {
                            EventName = eventinfo.Name,
                        };
                        trigger.Actions.Add(new InvokeAction(split[1]));
#else
                        // Attach a default 'Click' event to an action on the DataContext...
                        var trigger = new System.Windows.Interactivity.EventTrigger
                        {
                            EventName = eventinfo.Name,
                        };
                        trigger.Actions.Add(new InvokeAction(split[1]));

#endif
                        trigger.Attach(element);
                    }

                }
            }
        }

    }


}