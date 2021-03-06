﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Trine.Mobile.Components.Controls;
using Trine.Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MaterialEntry), typeof(MaterialEntryRenderer))]
namespace Trine.Mobile.iOS.Renderers
{
    public class MaterialEntryRenderer : EntryRenderer
    {
        private CALayer _line;


        public MaterialEntryRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            _line = null;

            if (Control == null || e.NewElement == null)
                return;

            Control.BorderStyle = UITextBorderStyle.None;

            // TODO: 
            // Control.Layer.AddSublayer(_line);
        }
    }
}