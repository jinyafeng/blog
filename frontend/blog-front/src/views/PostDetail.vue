<template>
  <div class="post-detail">
    <el-button @click="$router.go(-1)" type="text" style="margin-bottom: 20px">← 返回列表</el-button>
    <div class="post-header">
      <h1>{{ post.title }}</h1>
      <div class="post-meta">
        <el-tag type="success">{{ post.category?.name }}</el-tag>
        <span>📅 {{ new Date(post.createdAt).toLocaleDateString() }}</span>
        <span>💬 {{ post.comments?.length || 0 }} 条评论</span>
      </div>
      <div class="post-tags" style="margin: 15px 0">
        <el-tag v-for="tag in post.tags" :key="tag" type="info">{{ tag }}</el-tag>
      </div>
    </div>
    <el-divider></el-divider>
    <div class="post-content" v-html="post.content"></div>
    <el-divider></el-divider>
    <div class="comment-section">
      <h3>评论 ({{ comments.length }})</h3>
      <el-card v-for="comment in comments" :key="comment.id" class="comment-card">
        <div class="comment-header">
          <strong>{{ comment.authorName }}</strong>
          <span class="comment-time">{{ new Date(comment.createdAt).toLocaleString() }}</span>
        </div>
        <p class="comment-content">{{ comment.content }}</p>
      </el-card>
      <el-card class="comment-form">
        <h4>发表评论</h4>
        <el-form :model="newComment" label-width="80px">
          <el-form-item label="昵称">
            <el-input v-model="newComment.authorName" placeholder="请输入您的昵称"></el-input>
          </el-form-item>
          <el-form-item label="邮箱">
            <el-input v-model="newComment.authorEmail" placeholder="选填，用于通知回复"></el-input>
          </el-form-item>
          <el-form-item label="内容">
            <el-input type="textarea" v-model="newComment.content" :rows="4" placeholder="请输入评论内容"></el-input>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="submitComment">提交评论</el-button>
          </el-form-item>
        </el-form>
      </el-card>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'
const route = useRoute()
const post = ref({})
const comments = ref([])
const newComment = ref({
  authorName: '',
  authorEmail: '',
  content: '',
  blogPostId: 0
})

const fetchPost = async () => {
  const res = await fetch(`api/BlogPosts/slug/${route.params.slug}?renderMarkdown=true`)
  post.value = await res.json()
  newComment.value.blogPostId = post.value.id
  fetchComments()
}

const fetchComments = async () => {
  const res = await fetch(`api/Comments/post/${post.value.id}`)
  comments.value = await res.json()
}

const submitComment = async () => {
  if (!newComment.value.authorName || !newComment.value.content) {
    ElMessage.warning('昵称和评论内容不能为空')
    return
  }
  await fetch('api/Comments', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(newComment.value)
  })
  ElMessage.success('评论提交成功，等待审核后显示')
  newComment.value = {
    authorName: '',
    authorEmail: '',
    content: '',
    blogPostId: post.value.id
  }
}

onMounted(() => {
  fetchPost()
})
</script>

<style scoped>
.post-detail {
  max-width: 1000px;
  margin: 0 auto;
  padding: 20px;
}
.post-header h1 {
  font-size: 2rem;
  color: #303133;
  margin-bottom: 15px;
}
.post-meta {
  display: flex;
  gap: 15px;
  align-items: center;
  color: #909399;
  font-size: 0.9rem;
}
.post-content {
  line-height: 1.8;
  font-size: 1.1rem;
  color: #303133;
}
.post-content img {
  max-width: 100%;
  border-radius: 4px;
}
.post-content pre {
  background: #f5f7fa;
  padding: 15px;
  border-radius: 4px;
  overflow-x: auto;
}
.comment-section {
  margin-top: 40px;
}
.comment-card {
  margin-bottom: 15px;
}
.comment-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
}
.comment-time {
  color: #909399;
  font-size: 0.8rem;
}
.comment-content {
  color: #606266;
  line-height: 1.6;
}
.comment-form {
  margin-top: 30px;
}
</style>
