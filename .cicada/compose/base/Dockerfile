FROM mcr.microsoft.com/dotnet/sdk:7.0

ARG MOUNT_DIRECTORY=/app

ENV DEBIAN_FRONTEND=noninteractive

# Add jenkins user and group
ENV CICADA_JENKINS_USER=112
ENV CICADA_JENKINS_GROUP=119

RUN groupadd -r -f -g ${CICADA_JENKINS_GROUP} jenkins
RUN useradd -r -u ${CICADA_JENKINS_USER} -g ${CICADA_JENKINS_GROUP} jenkins

WORKDIR ${MOUNT_DIRECTORY}
