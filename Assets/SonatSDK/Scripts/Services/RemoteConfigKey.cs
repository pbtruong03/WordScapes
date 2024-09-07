using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable InconsistentNaming

public enum RemoteConfigKey
{
    start_level_show_interstitial = 0,
    interstitial_time_gap = 1,
    display_list_view_ads = 2,
    level_show_cross_app = 3,
    app_sku_ignore_list = 4,
    cross_app_data = 5,
    replace_level = 6, // not use, RemoteConfigDatabaseKey.level_database instead
    show_banner_enable = 7,
    seconds_to_dispose_ads = 8,
    use_open_ads_as_interstitial = 9,
    mediation_platform = 10,
    load_native_ads = 11,
    force_disable_test_ads = 12,
    start_level_show_banner = 13,
    start_level_have_event = 14, 
    difficulty = 15,
    level_show_no_ads_pack = 16,
    level_da_difficulty_separate = 17,
    enable_da_difficulty_separate = 18,
    start_level_show_offer = 19,
    
    admob_inter_ad_id = 20,
    admob_banner_ad_id = 21,
    admob_native_banner_ad_id = 22,
    admob_rewarded_ad_id = 23,
    admob_open_ad_id = 24,
    
    max_inter_ad_id = 30,
    max_banner_ad_id = 31,
    max_native_banner_ad_id = 32,
    max_rewarded_ad_id = 33,
    max_open_ad_id = 34,

    ironSource_inter_ad_id = 35,
    ironSource_banner_id = 36,
    ironSource_rewarded_ad_id = 37,
    ironSource_open_ad_id = 38,

    min_seconds_out_focus = 40,

    no_banner = 50,
    show_app_open_ads = 51,

    policy_url = 100,
    more_game_url = 101,
    fan_page_url = 102,
    notifications = 103,
    notify_day_title= 104,
    notify_day_content = 105,
    notify_week_title = 106,
    notify_week_content = 107,
    game_play_level_log = 108,
    game_play_level_pass_to_rate = 109,
    undo_default = 110,
    remote_enable = 111,
    internet_connection = 112,
    internet_connection_level = 113,
    gdpr_reset = 114,
    gdpr_ignore = 115,
    gdpr_force = 116,
    start_gdpr_date = 117,

    enabled_pam = 149,
    by_level_condition_interstitial_time_gap = 150,
    
    by_level_condition_admob_banner_id = 151,
    by_level_condition_admob_interstitial_id = 152,
    by_level_condition_admob_rewarded_id = 153,
    by_level_condition_admob_open_id = 154,
    by_level_condition_admob_native_id = 155,
    //
    by_level_condition_max_banner_id = 160,
    by_level_condition_max_interstitial_id = 161,
    by_level_condition_max_rewarded_id = 162,
    by_level_condition_max_open_id = 163,
    by_level_condition_max_native_id= 164,

    by_level_condition_iron_source_banner_id = 165,
    by_level_condition_iron_source_interstitial_id = 166,
    by_level_condition_iron_source_rewarded_id = 167,
    by_level_condition_iron_source_open_id = 168,
    by_level_condition_iron_source_native_id = 169,

    pam_admob_banner_id = 171,
    pam_admob_interstitial_id = 172,
    pam_admob_rewarded_id = 173,
    pam_admob_open_id = 174,
    pam_admob_native_id = 175,
    //
    pam_max_banner_id = 180,
    pam_max_interstitial_id = 181,
    pam_max_rewarded_id = 182,
    pam_max_open_id = 183,
    pam_max_native_id= 184,

    pam_iron_source_banner_id = 190,
    pam_iron_source_interstitial_id = 191,
    pam_iron_source_rewarded_id = 192,
    pam_iron_source_open_id = 193,
    pam_iron_source_native_id = 194,


    default_skin = 250,
    start_level_consume_energy = 251,
    turn_off_focus_ads = 252,
    enable_energy = 253,
    start_level_x2 = 254,
    ignore_inter_by_log_name = 255,

    auto_check_internet = 300,
    check_internet_time_gap = 301,

    by_level_times_lose_show_interstitial = 302,
    level_show_pop_up_no_ads = 303,
    level_show_pop_up_no_ads_24h = 304,
    level_show_pop_up_no_ads_just_fun = 305,
    level_show_pop_up_exclusive_deal = 306,
    level_show_pop_up_buy_1_get_1 = 307,
    level_show_pop_up_weekend_sales = 308,
    level_show_pop_up_value_bundle = 309,
    level_show_pop_up_only_for_you = 310,
    level_show_pop_up_ratings = 311,
    level_show_pop_up_win_streak = 312,
    level_start_feature_win_streak = 313,
    level_show_pop_up_starter_pack = 314,
    level_show_pop_up_boat_race = 315,
    level_start_feature_boat_race = 316,
    level_show_pop_up_pass = 317,
    level_show_pop_up_fortune_wheel = 318,
    level_start_feature_fortune_wheel = 319,
    level_show_pop_up_daily_rewards = 320,
    level_start_feature_daily_rewards = 321,
    level_show_pop_up_endless_treasure = 322,
    max_app_open_pop_up = 323,
    remote_level_data = 324
}

public enum RemoteConfigDatabaseKey
{
    game_setting = 0,
    shop_database = 1,
    level_database = 2,
}