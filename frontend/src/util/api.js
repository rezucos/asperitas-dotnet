const baseUrl =
  process.env.NODE_ENV === 'development'
    ? 'http://localhost:8080/api'
    : `https://${window.location.hostname}/api`;

const methods = {
  get: async function (endpoint, token = null) {
    const options = {
      method: 'GET',
      headers: {
        ...(token && { Authorization: `Bearer ${token}` })
      }
    };

    const response = await fetch(`${baseUrl}/${endpoint}`, options);
    const json = await response.json();

    if (!response.ok) throw buildError(json);

    return json;
  },

  post: async function (endpoint, body, token = null) {
    const options = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` })
      },
      body: JSON.stringify(body)
    };

    const response = await fetch(`${baseUrl}/${endpoint}`, options);
    const json = await response.json();

    if (!response.ok) {
      throw buildError(json);
    }

    return json;
  },

  delete: async function (endpoint, token = null) {
    const options = {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` })
      }
    };

    const response = await fetch(`${baseUrl}/${endpoint}`, options);
    const json = await response.json();

    if (!response.ok) {
      if (response.status === 401) throw Error('unauthorized');
      throw buildError(json);
    }

    return json;
  }
};

function buildError (json) {
  const message = (json && json.message) ? json.message : 'Request failed';
  const error = new Error(message);
  if (json && Array.isArray(json.errors) && json.errors.length > 0) {
    error.errors = json.errors;
  }
  return error;
}

export async function login (username, password) {
  const json = await methods.post('auth/login', { username, password });
  return json.token;
}

export async function signup (username, password) {
  const json = await methods.post('auth/register', { username, password });
  return json.token;
}

export async function getPosts (category, token) {
  return await methods.get(category ? `posts?category=${category}` : "posts", token);
}

export async function getProfile (username, token) {
  return await methods.get(`users/${username}`, token);
}

export async function getPost (id, token) {
  return await methods.get(`posts/${id}`, token);
}

export async function createPost (body, token) {
  return await methods.post('posts', body, token);
}

export async function deletePost (id, token) {
  return await methods.delete(`posts/${id}`, token);
}

export async function createComment (postId, comment, token) {
  return await methods.post(`posts/${postId}/comment`, comment, token);
}

export async function deleteComment (postId, commentId, token) {
  return await methods.delete(`posts/${postId}/comment/${commentId}`, token);
}

export async function castVote (id, vote, token) {
  const voteTypes = {
    '1': 'upvote',
    '0': 'unvote',
    '-1': 'downvote'
  };

  const voteType = voteTypes[vote];

  return await methods.post(`posts/${id}/${voteType}`, {}, token);
}
