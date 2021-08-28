<template>
  <page-layout :avatar="currUser.avatar">
    <div slot="headerContent">
      <div class="title">{{ currUser.petName }}，{{ currUser.userPhone }}</div>
      <!-- <div>{{currUser.position[lang]}}</div> -->
      <div>.net 优雅师 | 风风科技-计算服务事业群-移动出行平台部</div>
    </div>
    <template slot="extra">
      <head-info class="split-right" title="合作车场" content="12"/>
      <head-info class="split-right" title="今日交易笔数" content='2365' />
      <head-info class="split-right" title="今日交易流水" content='44869'  />
    </template>
    <template>
      <a-row style="margin: 0 -12px">
        <a-col
          style="padding: 0 12px"
          :xl="16"
          :lg="24"
          :md="24"
          :sm="24"
          :xs="24"
        >
          <a-card
            class="project-list"
            style="margin-bottom: 24px"
            :bordered="false"
            title="运行中的车场"
            :body-style="{ padding: 0 }"
          >
            <router-link :to="'/park/workplace'" slot="extra"
              >全部车场</router-link>
            <div>
              <a-card-grid :key="i" v-for="(item, i) in this.park">
                <a-card :bordered="false" :body-style="{ padding: 0 }">
                  <!-- :description="item.desc" -->
                  <a-card-meta>
                    <div slot="title" class="card-title">
                      <a-avatar
                        size="large"
                        src="https://gw.alipayobjects.com/zos/rmsportal/jZUIxmJycoymBprLOUbT.png"
                      />

                      <a style="padding-left: 10px" class="group" href="/#/">{{
                        item.parkName
                      }}</a>
                    </div>
                  </a-card-meta>
                  <div class="project-item">
                    <span class="group">{{ item.parkFullName }}</span>
                  </div>
                  <div class="project-item">
                    <span class="group"
                      >{{ item.province }} {{ item.city }} {{ item.region }}
                      {{ item.street }} {{ item.parkAddress }}
                    </span>
                  </div>
                </a-card>
              </a-card-grid>
            </div>
          </a-card>
          <a-card :loading="loading" :title="$t('dynamic')" :bordered="false">
            <a-list>
              <a-list-item :key="index" v-for="(item, index) in activities">
                <a-list-item-meta>
                  <a-avatar slot="avatar" :src="item.user.avatar" />
                  <div slot="title" v-html="item.template" />
                  <div slot="description">9小时前</div>
                </a-list-item-meta>
              </a-list-item>
            </a-list>
          </a-card>
        </a-col>
        <a-col
          style="padding: 0 12px"
          :xl="8"
          :lg="24"
          :md="24"
          :sm="24"
          :xs="24"
        >
          <a-card
            :title="$t('access')"
            style="margin-bottom: 24px"
            :bordered="false"
            :body-style="{ padding: 0 }"
          >
            <div
              class="item-group"
              :key="index"
              v-for="(item, index) in fastnav"
            >            
              <router-link :to="item.navigationUrl">{{
                item.title
              }}</router-link>
            </div>
            <a-button size="small" type="primary" ghost icon="plus">{{
              $t("add")
            }}</a-button>
          </a-card>
          <a-card
            :loading="loading"
            :title="`XX ${$t('degree')}`"
            style="margin-bottom: 24px"
            :bordered="false"
            :body-style="{ padding: 0 }"
          >
            <div style="min-height: 400px">
              <radar />
            </div>
          </a-card>
          <a-card :loading="loading" :title="$t('team')" :bordered="false">
            <div class="members">
              <a-row>
                <a-col :span="12" v-for="(item, index) in teams" :key="index">
                  <a>
                    <a-avatar size="small" :src="item.avatar" />
                    <span class="member">{{ item.name }}</span>
                  </a>
                </a-col>
              </a-row>
            </div>
          </a-card>
        </a-col>
      </a-row>
    </template>
  </page-layout>
</template>
<script>
import PageLayout from "@/layouts/PageLayout";
import HeadInfo from "@/components/tool/HeadInfo";
import Radar from "@/components/chart/Radar";
import { mapState } from "vuex";
import { gettop6park, fastnav,basesStatistics } from "@/services/workplace/index";
export default {
  name: "WorkPlace",
  components: { Radar, HeadInfo, PageLayout },
  i18n: require("./i18n"),
  data() {
    return {
      projects: [],
      park: [],
      loading: true,
      basesStatistics:{
           parkCount:'',
           tranCount:'',
           tranAmount:''
      },
      fastnav: [],
      activities: [],
      teams: [],
      welcome: {
        timeFix: "",
        message: "",
      },
    };
  },
  computed: {
    ...mapState("account", { currUser: "user" }),
    ...mapState("setting", ["lang"]),
  },
  created() {
  
    basesStatistics().then((res)=>(this.basesStatistics=res.data.response))
    gettop6park().then((res) => {
      this.park = res.data.response;
      console.log(this.park);
    });
    fastnav().then((res) => {
      this.fastnav = res.data.response;
    });
   
  },
};
</script>

<style lang="less">
@import "index";
</style>
