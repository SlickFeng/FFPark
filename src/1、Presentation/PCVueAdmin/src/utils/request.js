import axios from 'axios'
import Cookie from 'js-cookie'

// 跨域认证信息 header 名
const xsrfHeaderName = 'Authorization'

const refreshToken = 'ffparkrefreshToken'
//const accessToken = 'ffparkaccessToken'
axios.defaults.timeout = 5000
axios.defaults.withCredentials = true
axios.defaults.xsrfHeaderName = xsrfHeaderName
axios.defaults.xsrfCookieName = xsrfHeaderName
axios.defaults.headers['Content-Type'] = 'application/json;charset=utf-8'
axios.defaults.baseURL='https://localhost:5001/api/'

// http method
const METHOD = {
  GET: 'get',
  POST: 'post'
}

/**
 * axios请求
 * @param url 请求地址
 * @param method {METHOD} http method
 * @param params 请求参数
 * @returns {Promise<AxiosResponse<T>>}
 */
async function request(url, method, params, config) {
  switch (method) {
    case METHOD.GET:
      return axios.get(url, { params, ...config })
    case METHOD.POST:
      return axios.post(url, params, config)
    default:
      return axios.get(url, { params, ...config })
  }
}
async function getToken() {
  return Cookie.get(xsrfHeaderName)
}

async function getRefreshToken() {
  return Cookie.get(xsrfHeaderName)
}
/**
 *  带有token 请求头的 get 请求
 * @param url 请求地址
 * @param params 请求参数
 */
async function requestGet(url, params) {
  return axios.get(url, params, {
    headers: {
      'Authorization': getToken()
    }
  })
}
/**
 * 带有token请求头的post 请求
 * @param {请求地址} url 
 * @param {请求参数} params 
 */
async function requestPost(url, params) {
  return axios.post(url, params, {
    headers: {
      'Authorization': getToken()
    }
  })
}


/**
 * 设置认证信息
 * @param auth {Object}
 * @param authType {AUTH_TYPE} 认证类型，默认：{AUTH_TYPE.BEARER}
 */
function setAuthorization(auth) {
  Cookie.set(xsrfHeaderName, 'Bearer ' + auth.accessToken, { expires: auth.expireIn })
  Cookie.set(refreshToken, auth.refreshToken, { expires: auth.expireIn })
}
/**
 * 移出认证信息
 * @param authType {AUTH_TYPE} 认证类型
 */
function removeAuthorization() {
  Cookie.remove(xsrfHeaderName)
  Cookie.remove(refreshToken)
}

/**
 * 检查认证信息
 * @param authType
 * @returns {boolean}
 */
function checkAuthorization() {
  if (Cookie.get(xsrfHeaderName))
    return true
  return false
}

/**
 * 加载 axios 拦截器
 * @param interceptors
 * @param options
 */
function loadInterceptors(interceptors, options) {
  const { request, response } = interceptors
  // 加载请求拦截器
  request.forEach(item => {
    let { onFulfilled, onRejected } = item
    if (!onFulfilled || typeof onFulfilled !== 'function') {
      onFulfilled = config => config
    }
    if (!onRejected || typeof onRejected !== 'function') {
      onRejected = error => Promise.reject(error)
    }
    axios.interceptors.request.use(
      config => onFulfilled(config, options),
      error => onRejected(error, options)
    )
  })
  // 加载响应拦截器
  response.forEach(item => {
    let { onFulfilled, onRejected } = item
    if (!onFulfilled || typeof onFulfilled !== 'function') {
      onFulfilled = response => response
    }
    if (!onRejected || typeof onRejected !== 'function') {
      onRejected = error => Promise.reject(error)
    }
    axios.interceptors.response.use(
      response => onFulfilled(response, options),
      error => onRejected(error, options)
    )
  })
}

/**
 * 解析 url 中的参数
 * @param url
 * @returns {Object}
 */
function parseUrlParams(url) {
  const params = {}
  if (!url || url === '' || typeof url !== 'string') {
    return params
  }
  const paramsStr = url.split('?')[1]
  if (!paramsStr) {
    return params
  }
  const paramsArr = paramsStr.replace(/&|=/g, ' ').split(' ')
  for (let i = 0; i < paramsArr.length / 2; i++) {
    const value = paramsArr[i * 2 + 1]
    params[paramsArr[i * 2]] = value === 'true' ? true : (value === 'false' ? false : value)
  }
  return params
}

export {
  METHOD,
  request,
  setAuthorization,
  removeAuthorization,
  checkAuthorization,
  loadInterceptors,
  parseUrlParams,
  requestPost,
  requestGet,
  getRefreshToken
}
