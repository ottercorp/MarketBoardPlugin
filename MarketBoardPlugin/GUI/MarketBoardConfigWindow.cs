// <copyright file="MarketBoardConfigWindow.cs" company="Florian Maunier">
// Copyright (c) Florian Maunier. All rights reserved.
// </copyright>

namespace MarketBoardPlugin
{
  using System;
  using System.Numerics;
  using Dalamud.Bindings.ImGui;
  using Dalamud.Interface.Windowing;
  using MarketBoardPlugin.Helpers;

  /// <summary>
  /// The market board config window.
  /// </summary>
  public class MarketBoardConfigWindow : Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MarketBoardConfigWindow"/> class.
    /// </summary>
    /// <param name="plugin">The <see cref="MBPlugin"/>.</param>
    public MarketBoardConfigWindow(MBPlugin plugin)
      : base("Market Board Config")
    {
      this.Flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoResize;
      this.Size = new Vector2(0, 0);

      this.Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
    }

    private MBPlugin Plugin { get; init; }

    /// <inheritdoc/>
    public override void Draw()
    {
      this.Checkbox("右键菜单集成", "启用或禁用右键菜单集成", this.Plugin.Config.ContextMenuIntegration, (v) => this.Plugin.Config.ContextMenuIntegration = v);

      this.Checkbox("显示金币图标", "是否显示金币图标", this.Plugin.Config.PriceIconShown, (v) => this.Plugin.Config.PriceIconShown = v);

      this.Checkbox("不包含税率", "启用后将不包含税率", this.Plugin.Config.NoGilSalesTax, (v) =>
      {
        this.Plugin.Config.NoGilSalesTax = v;
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
        this.Plugin.ResetMarketData();
      });

      this.Checkbox("禁用最近购买历史", "启用或禁用最近购买历史", this.Plugin.Config.RecentHistoryDisabled, (v) => this.Plugin.Config.RecentHistoryDisabled = v);

      this.Checkbox("自动查询选中物品", "自动查询选中超过 1 秒的物品.", this.Plugin.Config.WatchForHovered, (v) => this.Plugin.Config.WatchForHovered = v);

      this.Checkbox("隐藏 Ko-Fi 按钮", "启用或禁用 Ko-Fi 按钮", this.Plugin.Config.KofiHidden, (v) => this.Plugin.Config.KofiHidden = v);

      this.Checkbox("过滤模特条目", "将通过模特交易的物品条目过滤", this.Plugin.Config.FilterMannequinListings, (v) =>
      {
        this.Plugin.Config.FilterMannequinListings = v;
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
        this.Plugin.ResetMarketData();
      });

      var itemRefreshTimeout = this.Plugin.Config.ItemRefreshTimeout;
      ImGui.Text("物品缓存时长 (ms) :");
      ImGui.InputInt("###refreshTimeout", ref itemRefreshTimeout);
      if (this.Plugin.Config.ItemRefreshTimeout != itemRefreshTimeout)
      {
        this.Plugin.Config.ItemRefreshTimeout = itemRefreshTimeout;
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
      }

      var listingCount = this.Plugin.Config.ListingCount;
      ImGui.Text("上架物品显示数量:");
      ImGui.InputInt("###listingCount", ref listingCount);
      if (this.Plugin.Config.ListingCount != listingCount)
      {
        this.Plugin.Config.ListingCount = listingCount;
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
      }

      var historyCount = this.Plugin.Config.HistoryCount;
      ImGui.Text("最近购买历史显示数量:");
      ImGui.InputInt("###historyCount", ref historyCount);
      if (this.Plugin.Config.HistoryCount != historyCount)
      {
        this.Plugin.Config.HistoryCount = historyCount;
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
      }
    }

    private void Checkbox(string label, string description, bool oldValue, Action<bool> setter)
    {
      if (Utilities.Checkbox(label, description, oldValue, setter))
      {
        this.Plugin.PluginInterface.SavePluginConfig(this.Plugin.Config);
      }
    }
  }
}
