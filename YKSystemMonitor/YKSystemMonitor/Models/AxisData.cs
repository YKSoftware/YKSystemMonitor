namespace YKSystemMonitor.Models
{
    using YKToolkit.Bindings;

    /// <summary>
    /// 軸設定を表します。
    /// </summary>
    internal class AxisData : NotificationObject
    {
        #region 公開メソッド

        /// <summary>
        /// 横軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        public void SetRangeX(double min, double max)
        {
            SetRangeXCore(min, max);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// 横軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        /// <param name="step">間隔を指定します。</param>
        public void SetRangeX(double min, double max, double step)
        {
            SetRangeXCore(min, max, step);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// 縦軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        public void SetRangeY(double min, double max)
        {
            SetRangeYCore(min, max);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// 縦軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        /// <param name="step">間隔を指定します。</param>
        public void SetRangeY(double min, double max, double step)
        {
            SetRangeYCore(min, max, step);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// 第 2 主軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        public void SetRangeY2(double min, double max)
        {
            SetRangeY2Core(min, max);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// 第 2 主軸のレンジを設定します。
        /// </summary>
        /// <param name="min">最小値を指定します。</param>
        /// <param name="max">最大値を指定します。</param>
        /// <param name="step">間隔を指定します。</param>
        public void SetRangeY2(double min, double max, double step)
        {
            SetRangeY2Core(min, max, step);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// レンジを設定します。
        /// </summary>
        /// <param name="xMin">横軸最小値を指定します。</param>
        /// <param name="xMax">横軸最大値を指定します。</param>
        /// <param name="yMin">縦軸最小値を指定します。</param>
        /// <param name="yMax">縦軸最大値を指定します。</param>
        public void SetRange(double xMin, double xMax, double yMin, double yMax)
        {
            SetRangeXCore(xMin, xMax);
            SetRangeYCore(yMin, yMax);
            RaisePropertyChanged("");
        }

        /// <summary>
        /// レンジを設定します。
        /// </summary>
        /// <param name="xMin">横軸最小値を指定します。</param>
        /// <param name="xMax">横軸最大値を指定します。</param>
        /// <param name="xStep">横軸間隔を指定します。</param>
        /// <param name="yMin">縦軸最小値を指定します。</param>
        /// <param name="yMax">縦軸最大値を指定します。</param>
        /// <param name="yStep">縦軸間隔を指定します。</param>
        public void SetRange(double xMin, double xMax, double xStep, double yMin, double yMax, double yStep)
        {
            SetRangeXCore(xMin, xMax, xStep);
            SetRangeYCore(yMin, yMax, yStep);
            RaisePropertyChanged("");
        }

        #endregion 公開メソッド

        #region 公開プロパティ

        private double _xMin = 0;
        /// <summary>
        /// 横軸最小値を取得または設定します。
        /// </summary>
        public double XMin
        {
            get { return this._xMin; }
            set { SetProperty(ref this._xMin, value); }
        }

        private double _xMax = 100;
        /// <summary>
        /// 横軸最大値を取得または設定します。
        /// </summary>
        public double XMax
        {
            get { return this._xMax; }
            set { SetProperty(ref this._xMax, value); }
        }

        private double _xStep = 10;
        /// <summary>
        /// 横軸間隔を取得または設定します。
        /// </summary>
        public double XStep
        {
            get { return this._xStep; }
            set { SetProperty(ref this._xStep, value); }
        }

        private double _yMin = 0;
        /// <summary>
        /// 縦軸最小値を取得または設定します。
        /// </summary>
        public double YMin
        {
            get { return this._yMin; }
            set { SetProperty(ref this._yMin, value); }
        }

        private double _yMax = 1024;
        /// <summary>
        /// 縦軸最大値を取得または設定します。
        /// </summary>
        public double YMax
        {
            get { return this._yMax; }
            set { SetProperty(ref this._yMax, value); }
        }

        private double _yStep = 102.4;
        /// <summary>
        /// 縦軸間隔を取得または設定します。
        /// </summary>
        public double YStep
        {
            get { return this._yStep; }
            set { SetProperty(ref this._yStep, value); }
        }

        private double _y2Min = 0;
        /// <summary>
        /// 第 2 主軸最小値を取得または設定します。
        /// </summary>
        public double Y2Min
        {
            get { return this._y2Min; }
            set
            {
                if (SetProperty(ref this._y2Min, value))
                {
                    //if (this._y2Min < 0)
                    {
                        this._y2Min = 0;
                        this._y2Step = (this._y2Max - this._y2Min) / 10.0;
                        RaisePropertyChanged("");
                    }
                }
            }
        }

        private double _y2Max = 5000;
        /// <summary>
        /// 第 2 主軸最大値を取得または設定します。
        /// </summary>
        public double Y2Max
        {
            get { return this._y2Max; }
            set { SetProperty(ref this._y2Max, value); }
        }

        private double _y2Step = 500;
        /// <summary>
        /// 第 2 主軸間隔を取得または設定します。
        /// </summary>
        public double Y2Step
        {
            get { return this._y2Step; }
            set { SetProperty(ref this._y2Step, value); }
        }

        #endregion 公開プロパティ

        #region private メソッド

        private void SetRangeXCore(double min, double max)
        {
            this._xMin = min < max ? min : max;
            this._xMax = max < min ? min : max;
        }

        private void SetRangeXCore(double min, double max, double step)
        {
            SetRangeXCore(min, max);
            this._xStep = step > 0.0 ? step : (this._xMax - this._xMin) / 10.0;
        }

        private void SetRangeYCore(double min, double max)
        {
            this._yMin = min < max ? min : max;
            this._yMax = max < min ? min : max;
        }

        private void SetRangeYCore(double min, double max, double step)
        {
            SetRangeYCore(min, max);
            this._yStep = step > 0.0 ? step : (this._yMax - this.YMin) / 10.0;
        }

        private void SetRangeY2Core(double min, double max)
        {
            this._y2Min = min < max ? min : max;
            this._y2Max = max < min ? min : max;
        }

        private void SetRangeY2Core(double min, double max, double step)
        {
            SetRangeY2Core(min, max);
            this._y2Step = step > 0.0 ? step : (this._y2Max - this.Y2Min) / 10.0;
        }

        #endregion private メソッド
    }
}
