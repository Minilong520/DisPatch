const CURRENT_USER_KEY = "current-user";
const localStorage = window.localStorage;
const sessionStorage = window.sessionStorage;

/**
 * 设置当前用户信息
 */
export function setLoginUser(currentUser: any) {
    // 将用户属性在这里展开，方便查看系统都用到了那些用户属性
    const { id, name, avatar, token, permissions, ...rest } = currentUser;
    const userStr = JSON.stringify({
        id,           // 用户id 必须
        name,         // 用户名 必须
        avatar,       // 用头像 非必须
        token,        // 登录凭证 非必须 ajax请求有可能会用到，也许是cookie
        permissions,  // 用户权限
        ...rest,
    });

    sessionStorage.setItem(CURRENT_USER_KEY, userStr);
}


/**
 * 获取当前用户信息
 */
export function getUserInfo() {
    const loginUser = localStorage.getItem("user-login-info");
    return loginUser ? JSON.parse(loginUser) : null;
}
export function getLoginUser() {
    const localUser: any = getUserInfo();
    if (localUser) {
        return localUser;
    } else {
        const loginUser = sessionStorage.getItem(CURRENT_USER_KEY);

        return loginUser ? JSON.parse(loginUser) : null;
    }
}


