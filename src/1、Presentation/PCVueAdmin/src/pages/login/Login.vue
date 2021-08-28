<template>
  <common-layout>
    <div class="top">
      <div class="header">
        <img alt="logo" class="logo" src="@/assets/img/logo.png" />
        <span class="title">{{ systemName }}</span>
      </div>
      <div class="desc">.Net Core 开源停车场管理系统</div>
    </div>
    <div class="login">
      <a-form @submit="onSubmit" :form="form">
        <a-tabs
          size="large"
          :tabBarStyle="{ textAlign: 'center' }"
          style="padding: 0 2px"
        >
          <a-tab-pane tab="账户密码登录" key="1">
            <a-alert
              type="error"
              :closable="true"
              v-show="error"
              :message="error"
              showIcon
              style="margin-bottom: 24px"
            />
            <a-form-item>
              <a-input
                autocomplete="autocomplete"
                size="large"
                placeholder="admin"
                v-decorator="[
                  'appkey',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入账户名',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="user" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-input
                size="large"
                placeholder="123456"
                autocomplete="autocomplete"
                type="password"
                v-decorator="[
                  'appsecret',
                  {
                    rules: [
                      {
                        required: true,
                        message: '请输入密码',
                        whitespace: true,
                      },
                    ],
                  },
                ]"
              >
                <a-icon slot="prefix" type="lock" />
              </a-input>
            </a-form-item>
          </a-tab-pane>
        </a-tabs>
        <a-form-item>
          <a-button
            :loading="logging"
            style="width: 100%; margin-top: 24px"
            size="large"
            htmlType="submit"
            type="primary"
            >登录</a-button
          >
        </a-form-item>
      </a-form>
    </div>
  </common-layout>
</template>

<script>
import CommonLayout from "@/layouts/CommonLayout";
import { login, getUserInfo } from "@/services/user";
import { setAuthorization } from "@/utils/request";
// import {loadRoutes} from '@/utils/routerUtil'
import { mapMutations } from "vuex";

export default {
  name: "Login",
  components: { CommonLayout },
  data() {
    return {
      logging: false,
      error: "",
      form: this.$form.createForm(this),
    };
  },
  computed: {
    systemName() {
      return this.$store.state.setting.systemName;
    },
  },
  methods: {
    //...mapMutations('account', ['setUser', 'setPermissions', 'setRoles']),
    ...mapMutations("account", ["setUser"]),
    onSubmit(e) {
      e.preventDefault();
      this.form.validateFields((err) => {
        if (!err) {
          this.logging = true;
          const appkey = this.form.getFieldValue("appkey");
          const appsecret = this.form.getFieldValue("appsecret");
          login(appkey, appsecret).then(this.afterLogin);
        }
      });
    },
    afterLogin(res) {
      this.logging = false;
      const loginRes = res.data;
      if (loginRes.status == 200) {
        //   const {user, permissions, roles} = loginRes.data
        getUserInfo().then((res) => {
          this.setUser(res.data.response);
        });
        // this.setPermissions(permissions)
        // this.setRoles(roles)
        setAuthorization(loginRes.response);
        this.$router.push("/dashboard/workplace");

        // // 获取路由配置
        // getRoutesConfig().then(result => {
        //   const routesConfig = result.data.data
        //   loadRoutes(routesConfig)
        //   this.$router.push('/dashboard/workplace')
        //   this.$message.success(loginRes.message, 3)
        // })
      } else {
        this.error = loginRes.msg;
      }
    },
  },
};
</script>

<style lang="less" scoped>
.common-layout {
  .top {
    text-align: center;
    .header {
      height: 44px;
      line-height: 44px;
      a {
        text-decoration: none;
      }
      .logo {
        height: 44px;
        vertical-align: top;
        margin-right: 16px;
      }
      .title {
        font-size: 33px;
        color: @title-color;
        font-family: "Myriad Pro", "Helvetica Neue", Arial, Helvetica,
          sans-serif;
        font-weight: 600;
        position: relative;
        top: 2px;
      }
    }
    .desc {
      font-size: 14px;
      color: @text-color-second;
      margin-top: 12px;
      margin-bottom: 40px;
    }
  }
  .login {
    width: 368px;
    margin: 0 auto;
    @media screen and (max-width: 576px) {
      width: 95%;
    }
    @media screen and (max-width: 320px) {
      .captcha-button {
        font-size: 14px;
      }
    }
    .icon {
      font-size: 24px;
      color: @text-color-second;
      margin-left: 16px;
      vertical-align: middle;
      cursor: pointer;
      transition: color 0.3s;

      &:hover {
        color: @primary-color;
      }
    }
  }
}
</style>
