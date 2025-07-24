# GPTAvatar VR Port - Hackathon PRD

## Project Overview

**Goal:** Port the existing GPTAvatar 3D AI chatbot from desktop to VR (Quest 3) in 40 hours

**Current State:** Working desktop Unity app with AI conversation pipeline (Whisper → GPT-4 → TTS → Character)

**Target:** Native Quest 3 VR experience with natural conversation interaction

## Success Criteria

### Minimum Viable Product (MVP)
- [ ] Single AI character conversation in VR
- [ ] Voice recording via VR controllers  
- [ ] Character responds with speech and animation
- [ ] Stable Quest Link experience
- [ ] Basic world-space UI

### Stretch Goals
- [ ] Native Quest 3 build (no PC tethering)
- [ ] Realtime conversation streaming
- [ ] Enhanced VR-specific interactions
- [ ] Multiple character selection

## Technical Requirements

### Core Systems
- **Unity Version:** 2022.2+ (existing)
- **VR Platform:** Quest 3 via Quest Link → Native Quest
- **AI Pipeline:** OpenAI Whisper + GPT-4 + ElevenLabs/Google TTS
- **Character:** Single character (TBD: Seth vs Teacher vs Big Burger)
- **Interaction:** XR Interaction Toolkit

### Performance Targets
- **Quest Link:** 90fps stable
- **Quest Native:** 72fps minimum, 90fps target
- **Latency:** <3s from voice input to character response

## Phase Breakdown

## Phase 1: Baseline ✅ (DONE)
- [x] Desktop version working
- [x] All AI APIs functional
- [x] Character animations and lip-sync working

---

## Phase 2: VR Foundation (4-6 hours)
**Goal:** Get basic VR running via Quest Link

### Tasks
- [ ] **XR Setup** (2h)
  - [ ] Import XR Interaction Toolkit package
  - [ ] Replace main camera with XR Rig
  - [ ] Configure XR settings for Quest
  - [ ] Test Quest Link connectivity

- [ ] **Character Verification** (2h)
  - [ ] Verify character is visible in VR
  - [ ] Check character scale/positioning
  - [ ] Test audio playback in VR space
  - [ ] Verify animator states still work

- [ ] **Core AI Pipeline Test** (1-2h)
  - [ ] Test microphone input in VR
  - [ ] Verify API calls still function
  - [ ] Confirm TTS audio works in VR
  - [ ] Test basic conversation flow

### Success Criteria
- [ ] Can put on Quest headset and see the character
- [ ] Character still talks when triggered from desktop UI
- [ ] No major VR sickness/comfort issues

### Risk Mitigation
- Keep desktop controls as backup
- Don't modify existing AI pipeline yet

---

## Phase 3: VR Interaction (8-10 hours)
**Goal:** Replace desktop controls with VR interactions

### Tasks
- [ ] **World-Space UI Conversion** (4h)
  - [ ] Convert main Canvas to WorldSpace
  - [ ] Resize buttons for VR interaction
  - [ ] Position UI panels in comfortable VR space
  - [ ] Test UI readability and interaction

- [ ] **Controller Interaction** (3h)
  - [ ] Implement ray-cast pointing from controllers
  - [ ] Add controller button mapping for recording
  - [ ] Visual feedback for button hover/press
  - [ ] Haptic feedback for interactions

- [ ] **Voice Recording in VR** (2h)
  - [ ] Map record button to controller trigger
  - [ ] Visual recording indicator (red light/pulse)
  - [ ] Test microphone input quality
  - [ ] Status display for AI processing

- [ ] **Remove Desktop Dependencies** (1h)
  - [ ] Disable mouse/keyboard input
  - [ ] Remove or adapt desktop-only features
  - [ ] Clean up unused UI elements

### Success Criteria
- [ ] Can record voice using VR controller
- [ ] Character responds to VR-initiated conversation
- [ ] All essential UI accessible via controllers
- [ ] Comfortable interaction distances and angles

### Character Selection
**Decision Point:** Choose single character for focus
- **Seth:** Simplest setup, default voice
- **Teacher:** Most educational value, Japanese capability
- **Big Burger:** Most entertaining, unique personality

**Recommendation:** Start with Seth (simplest), switch if time allows

---

## Phase 4: Quest Native Optimization (10-12 hours)
**Goal:** Native Quest 3 build with optimized performance

### Tasks
- [ ] **Android Build Setup** (3h)
  - [ ] Configure Android build settings
  - [ ] Set up Quest-specific XR configuration
  - [ ] Test initial Android build deployment
  - [ ] Debug any Quest-specific issues

- [ ] **Performance Optimization** (5h)
  - [ ] Profile frame rate and identify bottlenecks
  - [ ] Optimize character model poly count
  - [ ] Reduce texture resolution if needed
  - [ ] Implement LOD system for character
  - [ ] Optimize materials for mobile GPU

- [ ] **Quest-Specific Features** (3h)
  - [ ] Configure spatial audio
  - [ ] Add comfort settings (locomotion, etc.)
  - [ ] Test hand tracking (if time allows)
  - [ ] Optimize for Quest 3 processor/memory

- [ ] **Testing & Bug Fixes** (1h)
  - [ ] Test full conversation flow on native Quest
  - [ ] Fix any performance issues
  - [ ] Verify audio quality on Quest speakers
  - [ ] Test extended play sessions for comfort

### Success Criteria
- [ ] Native Quest 3 build runs at 72fps+
- [ ] Full conversation pipeline works offline
- [ ] No major VR comfort issues
- [ ] Audio quality maintained

### Performance Budgets
- **Draw Calls:** <50 per frame
- **Triangles:** <100k total
- **Texture Memory:** <500MB
- **System Memory:** <1GB

---

## Phase 5: Experience Optimization (8-10 hours)
**Goal:** Polish the experience and add advanced features

### Tasks
- [ ] **Character Persona Enhancement** (3h)
  - [ ] Update character prompts for VR context
  - [ ] Add VR-specific conversation topics
  - [ ] Enhance character personality for immersion
  - [ ] Test conversation quality and naturalness

- [ ] **Realtime Conversation** (4h)
  - [ ] Research OpenAI Realtime API integration
  - [ ] Implement streaming conversation (if feasible)
  - [ ] OR optimize current pipeline for lower latency
  - [ ] Add conversation flow improvements

- [ ] **VR Experience Polish** (2h)
  - [ ] Add spatial audio enhancements
  - [ ] Improve visual feedback systems
  - [ ] Add environmental details for immersion
  - [ ] Final UX polish pass

- [ ] **Multiple Characters** (1h - if time)
  - [ ] Add character selection menu
  - [ ] Test character switching in VR
  - [ ] Verify all characters work properly

### Success Criteria
- [ ] Conversation feels natural and engaging
- [ ] Response times <2s (current) or realtime streaming
- [ ] Immersive VR experience
- [ ] Demo-ready polish level

---

## Risk Management

### High-Risk Items
1. **VR Motion Sickness:** Test frequently, adjust comfort settings
2. **Performance on Quest:** Profile early, optimize aggressively  
3. **Audio Quality:** Quest built-in vs headphones testing
4. **Realtime API:** Have fallback plan (optimize current pipeline)

### Fallback Plans
- **Lip-sync issues:** Comment out `#define CRAZY_MINNOW_PRESENT`
- **Quest Native problems:** Demo via Quest Link
- **Performance issues:** Reduce visual quality
- **Realtime conversation fails:** Polish current record/process flow

### Cut-able Features (if time runs short)
1. Lip-syncing (still have talking animations)
2. Multiple characters (perfect one is better than three buggy ones)
3. Hand tracking (controller pointing is sufficient)
4. Realtime conversation (current flow works)
5. Advanced VR comfort features

## Development Environment

### Required Tools
- Unity 2022.2+
- XR Interaction Toolkit
- Quest 3 headset + Link cable
- Meta Quest Developer Hub
- Android SDK/NDK for Quest builds

### API Keys Required
- OpenAI API key (Whisper + GPT-4)
- ElevenLabs API key (or Google TTS)

## Testing Strategy

### Phase 2-3: Quest Link Testing
- Test frequently with headset during development
- Verify comfort and interaction quality
- Keep desktop version as comparison baseline

### Phase 4: Native Quest Testing
- Test builds on actual Quest 3 hardware
- Monitor performance metrics
- Extended play sessions for comfort testing

### Phase 5: Experience Testing
- Full conversation demos
- Test with different users if possible
- Polish based on feedback

## Timeline Summary

| Phase | Duration | Goal |
|-------|----------|------|
| 1 | Done | Desktop baseline working |
| 2 | 4-6h | VR foundation via Quest Link |
| 3 | 8-10h | VR interaction and UI |
| 4 | 10-12h | Native Quest build + optimization |
| 5 | 8-10h | Polish and advanced features |
| **Total** | **30-38h** | **Leaves 2-10h buffer** |

## Success Metrics

### MVP Success
- [ ] Single character conversation works in VR
- [ ] Stable performance on Quest 3
- [ ] Intuitive VR interaction
- [ ] Demo-ready experience

### Stretch Success  
- [ ] Realtime conversation streaming
- [ ] Multiple character options
- [ ] Premium VR experience quality
- [ ] Ready for distribution

---

**Last Updated:** [Current Date]  
**Project Duration:** 40 hours  
**Target Platform:** Meta Quest 3  
**Team Size:** 1 developer 