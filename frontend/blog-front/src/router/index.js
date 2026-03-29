import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import PostDetail from '../views/PostDetail.vue'
import Admin from '../views/Admin.vue'
import AdminPosts from '../views/AdminPosts.vue'
import AdminEditPost from '../views/AdminEditPost.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/post/:slug',
    name: 'PostDetail',
    component: PostDetail
  },
  {
    path: '/admin',
    name: 'Admin',
    component: Admin,
    children: [
      {
        path: 'posts',
        name: 'AdminPosts',
        component: AdminPosts
      },
      {
        path: 'posts/edit/:id?',
        name: 'AdminEditPost',
        component: AdminEditPost
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory('/blog/'),
  routes
})

export default router
