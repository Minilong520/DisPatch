/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(initialState: { currentUser?: any } | undefined) {
  const { currentUser } = initialState ?? {};
  return {
    /** 管理员登录 */
    isAdmin: currentUser.userInfo?.permissions === 0,
    /** 是否登录 */
    isLogin: currentUser.userInfo?.permissions !== undefined
  };
}
