import Footer from '@/components/Footer';
import { LockOutlined, UserOutlined, } from '@ant-design/icons';
import { LoginForm, ProFormCheckbox, ProFormText, } from '@ant-design/pro-components';
import { useEmotionCss } from '@ant-design/use-emotion-css';
import { FormattedMessage, history, SelectLang, useIntl, Helmet, useModel } from '@umijs/max';
import { message, Tabs } from 'antd';
import Settings from '../../../../config/defaultSettings';
import React, { useState } from 'react';
import { http } from '@/commons/request';
import { setLoginUser } from '@/commons/userinfo';
import { getInitialState } from '@/app';

/** 多国语 */
const Lang = () => {
  const langClassName = useEmotionCss(({ token }) => {
    return {
      width: 42,
      height: 42,
      lineHeight: '42px',
      position: 'fixed',
      right: 16,
      borderRadius: token.borderRadius,
      ':hover': {
        backgroundColor: token.colorBgTextHover,
      },
    };
  });

  return (
    <div className={langClassName} data-lang>
      {SelectLang && <SelectLang />}
    </div>
  );
};

const Login: React.FC = () => {
  const [type, setType] = useState<string>('account');

  // 全局样式
  const containerClassName = useEmotionCss(() => {
    return {
      display: 'flex',
      flexDirection: 'column',
      height: '100vh',
      overflow: 'auto',
      backgroundImage: "url('https://mdn.alipayobjects.com/yuyan_qk0oxh/afts/img/V-_oS6r-i7wAAAAAAAAAAAAAFl94AQBr')",
      backgroundSize: '100% 100%',
    };
  });

  const intl = useIntl();
  const { initialState, setInitialState } = useModel('@@initialState');

  /** 登录 */
  const handleSubmit = async (value: any) => {
    http.post('/User/login', {
      userNo: value.username,
      password: value.password
    }, {
      headers: {
        Authorization: 'login'
      }
    }).then(res => {
      setLoginUser({
        userInfo: res.userInfo,
        token: res.token
      });

      //刷新用户配置
      getInitialState().then((res2: any) => {
        setInitialState({
          currentUser: res2.currentUser,
        })
      });
      console.log(initialState);

      //下次是否自动登录
      if (value.autoLogin) {
        window.localStorage.setItem('user-info', JSON.stringify(res));
      }

      /** 此方法会跳转到 redirect 参数所在的位置 */
      console.log(history);
      const lastHref = window.localStorage.getItem("last-href");
      history.push(lastHref || '/gantt');

      const defaultLoginSuccessMessage = intl.formatMessage({ id: 'pages.login.success', defaultMessage: '登录成功！', });
      message.success(defaultLoginSuccessMessage);

    }).catch(ex => {
      // const defaultLoginFailureMessage = intl.formatMessage({ id: 'pages.login.failure', defaultMessage: '登录失败，请重试！', });
      console.log(ex);
      // message.error(defaultLoginFailureMessage + ":" + ex.message || ex.Message);
    });
  }

  return (
    <div className={containerClassName}>
      <Helmet><title>{intl.formatMessage({ id: 'menu.login', defaultMessage: '登录页', })}- {Settings.title}</title></Helmet>
      <Lang />
      <div style={{ flex: '1', padding: '32px 0', }}>
        <LoginForm contentStyle={{ minWidth: 280, maxWidth: '75vw', }}
          logo={<img alt="logo" src="/logo.ico" />}
          title="DisPatch Project"
          //subTitle={intl.formatMessage({ id: 'pages.layouts.userLayout.title' })}
          initialValues={{ autoLogin: true, }}
          onFinish={async (values) => { await handleSubmit(values as API.LoginParams); }}>
          <Tabs activeKey={type} onChange={setType} centered
            items={[
              { key: 'account', label: intl.formatMessage({ id: 'pages.login.accountLogin.tab', defaultMessage: '账户密码登录', }) }
            ]} />

          {type === 'account' && (
            <>
              <ProFormText
                name="username"
                fieldProps={{
                  size: 'large',
                  prefix: <UserOutlined />,
                }}
                placeholder={intl.formatMessage({
                  id: 'pages.login.username.placeholder',
                  defaultMessage: '用户名: admin or user',
                })}
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.username.required"
                        defaultMessage="请输入用户名!"
                      />
                    ),
                  },
                ]}
              />
              <ProFormText.Password
                name="password"
                fieldProps={{
                  size: 'large',
                  prefix: <LockOutlined />,
                }}
                placeholder={intl.formatMessage({
                  id: 'pages.login.password.placeholder',
                  defaultMessage: '密码: ant.design',
                })}
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.password.required"
                        defaultMessage="请输入密码！"
                      />
                    ),
                  },
                ]}
              />
            </>
          )}

          <div style={{ marginBottom: 24, }}>
            <ProFormCheckbox noStyle name="autoLogin"><FormattedMessage id="pages.login.rememberMe" defaultMessage="自动登录" /></ProFormCheckbox>
          </div>
        </LoginForm>
      </div>
      <Footer />
    </div>
  );
};

export default Login;
