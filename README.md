## How to delopy
Procedure:
1. Run ``docker-compose build``
2. Run ``docker-comopse up``


## Limitation(s)
Currently the cron job for drawing the lottery is embedded in the same program with the api controller. While the corn job is necessary to run in sigle instance to perform the draw, the api controller cannot run in mutli instance and cannot scale up the it when needed.\
To improve the it, the draw lottery cron job could be sperated to a stand alone program or run the draw lottery task with other cron job scheduler, e.g. k8s cron job.
