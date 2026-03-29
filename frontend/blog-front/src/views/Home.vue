<template>
  <div class="home">
    <div class="header">
      <h1>我的个人博客</h1>
      <p>记录自己的所见，所思和所得</p>
    </div>
    <div class="post-list">
      <el-card v-for="post in posts" :key="post.id" class="post-card" @click="goToPost(post.slug)">
        <template #header>
          <div class="post-header">
            <span class="post-title">{{ post.title }}</span>
            <el-tag type="success" size="small">{{ post.category?.name }}</el-tag>
          </div>
        </template>
        <p class="post-excerpt">{{ post.excerpt || post.content.substring(0, 100) + '...' }}</p>
        <div class="post-meta">
          <span>📅 {{ new Date(post.createdAt).toLocaleDateString() }}</span>
          <span>💬 {{ post.comments?.length || 0 }} 条评论</span>
          <div class="post-tags">
            <el-tag v-for="tag in post.tags" :key="tag" size="small" type="info">{{ tag }}</el-tag>
          </div>
        </div>
      </el-card>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
const router = useRouter()
const posts = ref([])

const fetchPosts = async () => {
  const res = await fetch('api/BlogPosts')
  posts.value = await res.json()
}

const goToPost = (slug) => {
  router.push(`/post/${slug}`)
}

onMounted(() => {
  fetchPosts()
})
</script>

<style scoped>
.home {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}
.header {
  text-align: center;
  margin-bottom: 40px;
}
.header h1 {
  font-size: 2.5rem;
  margin-bottom: 10px;
  color: #303133;
}
.header p {
  color: #909399;
  font-size: 1.1rem;
}
.post-card {
  margin-bottom: 20px;
  cursor: pointer;
  transition: all 0.3s;
}
.post-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  transform: translateY(-2px);
}
.post-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.post-title {
  font-size: 1.3rem;
  font-weight: bold;
  color: #303133;
}
.post-excerpt {
  color: #606266;
  line-height: 1.6;
  margin: 15px 0;
}
.post-meta {
  display: flex;
  align-items: center;
  gap: 20px;
  color: #909399;
  font-size: 0.9rem;
}
.post-tags {
  display: flex;
  gap: 5px;
  margin-left: auto;
}
</style>
