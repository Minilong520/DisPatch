import { defineConfig } from 'umi';
import defaultSettings from './defaultSettings';

export default defineConfig({
  nodeModulesTransform: {
    type: 'none',
  },
  hash: true,
  layout: {
    locale: true,
    siderWidth: 208,
    ...defaultSettings,
  },
  routes: [
    { path: '/', component: '@/pages/Gantt/index', name: "甘特模式" },
  ],
  fastRefresh: {},
});
