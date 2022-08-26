**SRP 배칭** 

같은 쉐이더끼리 묶어서 드로우콜을 위한 설정을 줄인다 (SetPass Call을 획기적으로 줄인다!)

SRP 배치를 위하여 쉐이더에 SRP Compatible 세팅을 해야한다.

![화면 캡처 2022-08-26 195949](https://user-images.githubusercontent.com/110815479/186889886-55f7e7d9-c8a5-4c0a-a910-116e2607783e.png)

GPU Instancing의 인스턴스별 외형 변화(MaterialPropertyBlock)는 지원하지 않는다.  

<br/>



**인스턴싱**


동일한 머티리얼, 동일한 메쉬들을 하나의 드로우콜로 렌더링하는 기능이다
쉐이더에서 인스턴싱 세팅을 하거나, 아래와 같이 코드를 통한 인스턴싱이 가능하다

_### Graphics.DrawMeshInstanced()_




MaterialPropertyBlock을 통해 인스턴스별 (오브젝트별) 외형 변화를 줄 수 있다.

SkinnedMeshRenderer는 지원하지 않는다.

<br/>

**SRP배칭과 인스턴싱을 사용할 때 주의 사항**

GPU 인스턴싱은 [SRP 배처](https://docs.unity3d.com/kr/2022.1/Manual/SRPBatcher.html)와 호환되지 않습니다. SRP 배처는 GPU 인스턴싱보다 우선합니다. 게임 오브젝트가 SRP 배처와 호환되는 경우 Unity는 GPU 인스턴싱이 아닌 SRP 배처를 사용하여 렌더링합니다. 최적화 방법 우선 순위에 대한 자세한 내용은 [최적화 우선 순위](https://docs.unity3d.com/kr/2022.1/Manual/optimizing-draw-calls.html#optimization-priority)를 참조하십시오.



프로젝트에서 SRP 배처를 사용하고 게임 오브젝트에 GPU 인스턴싱을 사용하려는 경우 다음 중 하나를 수행할 수 있습니다.

[Graphics.DrawMeshInstanced](https://docs.unity3d.com/kr/2022.1/ScriptReference/Graphics.DrawMeshInstanced.html)를 사용합니다. 이 API는 게임 오브젝트 사용을 우회하고 지정된 파라미터를 사용하여 화면에 메시를 직접 드로우합니다.
SRP 배처 호환성을 수동으로 제거합니다. 수동으로 제거하는 방법에 대한 자세한 내용은 [의도적으로 호환성 제거](https://docs.unity3d.com/kr/2022.1/Manual/SRPBatcher.html#intentionally-removing-compatibility)를 참조하십시오.
