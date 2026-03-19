import React from 'react';
import styled from 'styled-components/macro';
import PostVoteContainer from './Vote/Container';
import PostContent from './Content';

const Wrapper = styled.div`
  display: flex;
  height: auto;
  background-color: ${props => props.theme.foreground};
`;

const Post = ({ id, score, commentsCount, currentUserVote, full, ...content }) => (
  <Wrapper>
    <PostVoteContainer id={id} currentUserVote={currentUserVote} score={score} />
    <PostContent
      showFullPost={full}
      id={id}
      commentCount={commentsCount}
      {...content}
    />
  </Wrapper>
);

export default Post;
