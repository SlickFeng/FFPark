<template>
  <a-card :body-style="{ padding: '24px 32px' }" :bordered="false">
    <a-form @submit="onSubmit" :form="form">
      <a-form-item
        label="登录名"
        :labelCol="{ span: 7 }"
        :wrapperCol="{ span: 10 }"
      >
      <a-alert
              type="error"
              :closable="true"
              v-show="error"
              :message="error"
              showIcon
              style="margin-bottom: 24px"
            />
        <a-input
          v-decorator="[
            'UserName',
            {
              rules: [
                {
                  required: true,
                  message: '请输入用户名',
                  whitespace: true,
                },
              ],
            },
          ]"
        />
      </a-form-item>

      <a-form-item
        label="密码"
        :labelCol="{ span: 7 }"
        :wrapperCol="{ span: 10 }"
      >
        <a-input
          v-decorator="[
            'UserPassWord',
            {
              rules: [
                {
                  required: true,
                  message: '请输入登录密码',
                  whitespace: true,
                },
              ],
            },
          ]"
          placeholder="不输入默认123456"
        />
      </a-form-item>

      <a-form-item
        label="姓名"
        :labelCol="{ span: 7 }"
        :wrapperCol="{ span: 10 }"
      >
        <a-input
          v-decorator="[
            'PetName',
            {
              rules: [
                {
                  required: true,
                  message: '请输入姓名',
                  whitespace: true,
                },
              ],
            },
          ]"
        />
      </a-form-item>
      <a-form-item
        label="手机号码"
        :labelCol="{ span: 7 }"
        :wrapperCol="{ span: 10 }"
      >
        <a-input
         
          v-decorator="[
            'UserPhone',
            {
              rules: [
                {
                  required: true,
                  message: '请输入手机号码',
                  whitespace: true,
                },
              ],
            },
          ]"
        />
      </a-form-item>
      <a-form-item
        label="用户类型"
        :labelCol="{ span: 7 }"
        :wrapperCol="{ span: 10 }"
      >
        <a-radio-group v-decorator="['UserType',{
              rules: [
                {
                  required: true,
                  message: '请选择用户类型',
                  whitespace: true,
                },
              ],
            }]">
          <a-radio checked="checked" value="a"> 平台 </a-radio>
          <a-radio value="b"> 车场 </a-radio>
        </a-radio-group>
      </a-form-item>
      <a-form-item
        style="margin-top: 24px"
        :wrapperCol="{ span: 10, offset: 7 }"
      >
        <a-button
          :loading="logging"
          size="large"
          htmlType="submit"
          type="primary"
          >提交</a-button
        >
        <a-button style="margin-left: 8px">重置</a-button>
      </a-form-item>
    </a-form>
  </a-card>
</template>
<script>
// import {  newbaseuser } from "@/services/user"; 
export default {
  name: "new",
  data() {
    return {
      logging: false,
      error: "",
      form: this.$form.createForm(this),
      // form: {
      //   UserName: "",
      //   UserPassWord: "",
      //   UserType: "",
      //   userExtension: {
      //     UserPhone: "",
      //     PetName: "",
      //   },
      // },
    };
  },
  computed: {},
  methods: {
    onSubmit(e) {
      e.preventDefault();
      this.form.validateFields((err) => {
        if (!err) {
          console.log('hello world')
         // this.logging = true;
          // newbaseuser('').then(res=>
          // {
          //    console.log(res.data)
          // })
        }
      });
    },
  },
};
</script>

<style scoped>
</style>
